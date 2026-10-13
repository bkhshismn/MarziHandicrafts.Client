window.marziTheme = {
    apply: (background, text, primary, textSecondary) => {
        const root = document.documentElement;
        root.style.setProperty('--marzi-background', background);
        root.style.setProperty('--marzi-text', text);
        root.style.setProperty('--marzi-primary', primary);
        root.style.setProperty('--marzi-text-secondary', textSecondary);
    }
};
