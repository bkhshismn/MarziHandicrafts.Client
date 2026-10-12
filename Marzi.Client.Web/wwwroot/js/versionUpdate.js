window.marziVersion = {
    async get() {
        try {
            const response = await fetch(`/app-version?check=${Date.now()}`, {
                cache: "no-store",
                headers: { "Cache-Control": "no-cache" }
            });

            if (!response.ok)
                return null;

            const data = await response.json();
            return data.version ?? null;
        } catch {
            return null;
        }
    },

    refresh() {
        const url = new URL(window.location.href);
        url.searchParams.set("_siteUpdate", Date.now().toString());
        window.location.replace(url.toString());
    },

    showNotice() {
        if (document.querySelector(".site-update-notice"))
            return;

        const notice = document.createElement("div");
        notice.className = "site-update-notice";
        notice.setAttribute("role", "status");

        const message = document.createElement("span");
        message.className = "site-update-message";
        message.textContent = "نسخهٔ تازهٔ سایت آماده است.";

        const updateButton = document.createElement("button");
        updateButton.className = "site-update-button";
        updateButton.type = "button";
        updateButton.textContent = "به‌روزرسانی";
        updateButton.addEventListener("click", () => this.refresh());

        const closeButton = document.createElement("button");
        closeButton.className = "site-update-close";
        closeButton.type = "button";
        closeButton.textContent = "×";
        closeButton.setAttribute("aria-label", "بستن پیام");
        closeButton.addEventListener("click", () => notice.remove());

        notice.append(message, updateButton, closeButton);
        document.body.append(notice);
    },

    async init() {
        let currentVersion = null;

        const checkVersion = async () => {
            const latestVersion = await this.get();
            if (!latestVersion)
                return;

            if (currentVersion === null) {
                currentVersion = latestVersion;
                return;
            }

            if (latestVersion !== currentVersion) {
                this.showNotice();
                window.clearInterval(pollId);
            }
        };

        const pollId = window.setInterval(checkVersion, 60_000);
        await checkVersion();
    }
};

void window.marziVersion.init();
