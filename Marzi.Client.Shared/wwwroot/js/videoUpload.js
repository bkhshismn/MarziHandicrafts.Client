// آپلود مستقیم فایل از مرورگر به API.
//
// چرا؟ Blazor Server فایل را تکه‌تکه از طریق SignalR می‌خواند و روی اندروید
// با خطای NotReadableError شکست می‌خورد. اینجا خود مرورگر فایل را می‌فرستد
// و هیچ بایتی از کانال Blazor عبور نمی‌کند.
//
// از XMLHttpRequest استفاده می‌کنیم (نه fetch) تا رویداد upload.onprogress
// در دسترس باشد؛ همان چیزی که نوار پیشرفت و سرعت را ممکن می‌کند.

function getInput(input) {
    if (!input || !input.files || input.files.length === 0) {
        return null;
    }

    return input.files[0];
}

export function getSelectedFile(input) {
    const file = getInput(input);

    if (!file) {
        return null;
    }

    return {
        name: file.name,
        size: file.size,
        type: file.type || ""
    };
}

// بررسی می‌کند مرورگر واقعاً می‌تواند محتوای فایل را بخواند.
export async function verifyReadable(input) {
    const file = getInput(input);

    if (!file) {
        return { ok: false, error: "NO_FILE" };
    }

    try {
        const probeSize = Math.min(file.size, 1024 * 1024);

        await file.slice(0, probeSize).arrayBuffer();

        return { ok: true, error: "" };
    }
    catch (error) {
        return { ok: false, error: String(error) };
    }
}

export function clearInput(input) {
    if (input) {
        input.value = "";
    }
}

async function canReachApi(url, token) {
    const headers = {};

    if (token) {
        headers["Authorization"] = "Bearer " + token;
    }

    try {
        const response = await fetch(url, { method: "GET", headers: headers });

        return response.ok || response.status === 401 || response.status === 403;
    }
    catch (error) {
        console.error("[VIDEO UPLOAD] connectivity check failed:", error);

        return false;
    }
}

export async function uploadVideo(
    input,
    url,
    token,
    title,
    description,
    order,
    isFree,
    healthUrl,
    dotNetRef) {

    const file = getInput(input);

    if (!file) {
        return { status: 0, body: "NO_FILE_SELECTED" };
    }

    if (healthUrl && !(await canReachApi(healthUrl, token))) {
        return { status: -2, body: "NETWORK_UNREACHABLE" };
    }

    const data = new FormData();
    data.append("Title", title ?? "");
    data.append("Description", description ?? "");
    data.append("Order", String(order));
    data.append("IsFree", isFree ? "true" : "false");
    data.append("VideoFile", file, file.name);

    return new Promise((resolve) => {
        const xhr = new XMLHttpRequest();

        xhr.open("POST", url, true);

        if (token) {
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        }

        xhr.upload.onprogress = function (event) {
            if (!dotNetRef) {
                return;
            }

            const total = event.lengthComputable ? event.total : file.size;

            try {
                const result = dotNetRef.invokeMethodAsync(
                    "OnUploadProgress",
                    event.loaded,
                    total);

                if (result && typeof result.catch === "function") {
                    result.catch(function () { });
                }
            }
            catch (error) {
                // اگر مدار قطع شده باشد فقط نادیده می‌گیریم.
            }
        };

        xhr.onload = function () {
            resolve({ status: xhr.status, body: xhr.responseText || "" });
        };

        xhr.onerror = function () {
            resolve({ status: -1, body: "NETWORK_ERROR" });
        };

        xhr.onabort = function () {
            resolve({ status: -4, body: "ABORTED" });
        };

        xhr.ontimeout = function () {
            resolve({ status: -3, body: "TIMEOUT" });
        };

        xhr.send(data);
    });
}
