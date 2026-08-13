let players = new WeakMap();

export async function initialize(video, url, token) {
    if (!video) {
        throw new Error("Video element is required.");
    }

    dispose(video);

    if (window.Hls && window.Hls.isSupported()) {
        const hls = new window.Hls({
            enableWorker: true,
            xhrSetup: function (xhr) {
                xhr.setRequestHeader(
                    "Authorization",
                    `Bearer ${token}`
                );
            }
        });

        hls.loadSource(url);
        hls.attachMedia(video);

        hls.on(window.Hls.Events.ERROR, function (event, data) {
            console.error("HLS error:", data);

            if (data.fatal) {
                switch (data.type) {
                    case window.Hls.ErrorTypes.NETWORK_ERROR:
                        hls.startLoad();
                        break;

                    case window.Hls.ErrorTypes.MEDIA_ERROR:
                        hls.recoverMediaError();
                        break;

                    default:
                        hls.destroy();
                        break;
                }
            }
        });

        players.set(video, hls);

        return;
    }

    // Safari / native HLS
    if (video.canPlayType("application/vnd.apple.mpegurl")) {
        video.src = url;

        return;
    }

    throw new Error(
        "HLS is not supported by this browser."
    );
}


export function dispose(video) {
    if (!video) {
        return;
    }

    const player = players.get(video);

    if (player) {
        player.destroy();
        players.delete(video);
    }

    video.removeAttribute("src");
    video.load();
}