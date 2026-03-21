// Tailwind Configuration
tailwind.config = {
    darkMode: "class",
    theme: {
        extend: {
            colors: {
                primary: "#F97316",
                "background-light": "#F3F4F6",
                "background-dark": "#111827",
            },
            fontFamily: {
                display: ["Inter", "sans-serif"],
            },
            borderRadius: {
                DEFAULT: "8px",
            },
        },
    },
};

// Dark Mode Preference Check
if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
    // document.documentElement.classList.add('dark');
}
