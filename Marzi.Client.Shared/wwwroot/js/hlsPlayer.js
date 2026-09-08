let players = new WeakMap();

export async function initialize(video, url, token, dotNetHelper) {
    if (!video) {
        throw new Error("Video element is required.");
    }

    dispose(video);

    const updateVideoRatio = () => {
        if (video.videoWidth > 0 && video.videoHeight > 0) {
            video.parentElement?.style.setProperty(
                "--video-ratio",
                `${video.videoWidth} / ${video.videoHeight}`
            );
        }
    };

    video.addEventListener("loadedmetadata", updateVideoRatio);

    // ============================================================
    // Video Ended
    // ============================================================

    video.addEventListener(
        "ended",
        async function () {

            console.log(
                "[VIDEO PLAYER] Video ended."
            );

            if (!dotNetHelper) {
                console.warn(
                    "[VIDEO PLAYER] dotNetHelper is null."
                );

                return;
            }

            try {

                await dotNetHelper.invokeMethodAsync(
                    "VideoEnded"
                );

                console.log(
                    "[VIDEO PLAYER] VideoEnded callback completed."
                );

            }
            catch (error) {

                console.error(
                    "[VIDEO PLAYER] VideoEnded callback failed:",
                    error
                );
            }

        },
        {
            once: true
        }
    );


    // ============================================================
    // HLS.js
    // ============================================================

    if (window.Hls && window.Hls.isSupported()) {

        const hls = new window.Hls({

            enableWorker: true,

            xhrSetup: function (xhr) {

                if (token) {

                    xhr.setRequestHeader(
                        "Authorization",
                        `Bearer ${token}`
                    );
                }
            }
        });


        hls.loadSource(url);

        hls.attachMedia(video);

        hls.on(window.Hls.Events.MANIFEST_PARSED, updateVideoRatio);


        hls.on(
            window.Hls.Events.ERROR,
            function (event, data) {

                console.error(
                    "[HLS ERROR]",
                    data
                );


                if (data.fatal) {

                    switch (data.type) {

                        case window.Hls.ErrorTypes.NETWORK_ERROR:

                            console.log(
                                "[HLS] Recovering network error..."
                            );

                            hls.startLoad();

                            break;


                        case window.Hls.ErrorTypes.MEDIA_ERROR:

                            console.log(
                                "[HLS] Recovering media error..."
                            );

                            hls.recoverMediaError();

                            break;


                        default:

                            console.error(
                                "[HLS] Fatal error. Destroying player."
                            );

                            hls.destroy();

                            break;
                    }
                }
            }
        );


        players.set(
            video,
            hls
        );


        console.log(
            "[VIDEO PLAYER] HLS initialized successfully."
        );


        return;
    }


    // ============================================================
    // Safari / Native HLS
    // ============================================================

    if (
        video.canPlayType(
            "application/vnd.apple.mpegurl"
        )
    ) {

        video.src = url;

        console.log(
            "[VIDEO PLAYER] Native HLS initialized successfully."
        );

        return;
    }


    throw new Error(
        "HLS is not supported by this browser."
    );
}


// ============================================================
// Get current playback time
// ============================================================

export function getCurrentTime(video) {

    if (!video) {
        return 0;
    }


    const currentTime =
        video.currentTime;


    if (
        typeof currentTime !== "number" ||
        !Number.isFinite(currentTime)
    ) {

        return 0;
    }


    return currentTime;
}


// ============================================================
// Set playback time
// ============================================================

export function setCurrentTime(video, seconds) {

    if (!video) {
        return;
    }


    if (
        typeof seconds !== "number" ||
        !Number.isFinite(seconds) ||
        seconds < 0
    ) {

        return;
    }


    const setTime = function () {

        try {

            if (
                Number.isFinite(video.duration) &&
                video.duration > 0
            ) {

                video.currentTime =
                    Math.min(
                        seconds,
                        Math.max(
                            0,
                            video.duration - 0.5
                        )
                    );
            }
            else {

                video.currentTime =
                    seconds;
            }

        }
        catch (error) {

            console.warn(
                "Could not set video currentTime:",
                error
            );
        }
    };


    if (video.readyState >= 1) {

        setTime();

        return;
    }


    video.addEventListener(
        "loadedmetadata",
        setTime,
        {
            once: true
        }
    );
}


// ============================================================
// Dispose
// ============================================================

export function dispose(video) {

    if (!video) {
        return;
    }


    const player =
        players.get(video);


    if (player) {

        player.destroy();

        players.delete(video);
    }


    video.removeAttribute("src");

    video.load();
}