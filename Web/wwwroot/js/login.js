document.addEventListener("DOMContentLoaded", () => {

    // ===== PASSWORD TOGGLE =====
    const togglePassword = document.getElementById("togglePassword");
    const passwordInput = document.getElementById("password");

    if (togglePassword && passwordInput) {

        togglePassword.addEventListener("click", () => {

            const isHidden = passwordInput.type === "password";

            passwordInput.type = isHidden ? "text" : "password";

            togglePassword.querySelector("span").textContent =
                isHidden ? "visibility_off" : "visibility";
        });

    }


    // ===== LOGIN IMAGE SLIDER =====
    const slider = document.getElementById("loginSlider");

    if (!slider) return;

    const images = [
        "/images/login/login1.jpg",
        "/images/login/login2.jpg",
        "/images/login/login3.jpg"
    ];

    let index = 0;

    setInterval(() => {

        index = (index + 1) % images.length;

        slider.style.opacity = 0;

        setTimeout(() => {
            slider.src = images[index];
            slider.style.opacity = 1;
        }, 500);

    }, 4000);

});