document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("registerForm");

    const passwordInput = document.getElementById("Password");

    const lengthRule = document.getElementById("lengthRule");
    const numberRule = document.getElementById("numberRule");
    const upperRule = document.getElementById("upperRule");

    passwordInput.addEventListener("input", function () {

        const password = passwordInput.value;

        // length check
        if (password.length >= 6) {
            lengthRule.classList.remove("text-gray-400");
            lengthRule.classList.add("text-green-500");
        } else {
            lengthRule.classList.add("text-gray-400");
            lengthRule.classList.remove("text-green-500");
        }

        // number check
        if (/\d/.test(password)) {
            numberRule.classList.remove("text-gray-400");
            numberRule.classList.add("text-green-500");
        } else {
            numberRule.classList.add("text-gray-400");
            numberRule.classList.remove("text-green-500");
        }

        // uppercase check
        if (/[A-Z]/.test(password)) {
            upperRule.classList.remove("text-gray-400");
            upperRule.classList.add("text-green-500");
        } else {
            upperRule.classList.add("text-gray-400");
            upperRule.classList.remove("text-green-500");
        }

    });


    let isEmailValid = true;
    const emailInput = document.getElementById("Email");
    const emailError = document.getElementById("emailError");

    emailInput.addEventListener("blur", async function () {
        const email = emailInput.value.trim();
        if (!email) {
            emailError.classList.add("hidden");
            isEmailValid = false;
            return;
        }

        if (!validateEmail(email)) {
            emailError.textContent = "Email không hợp lệ.";
            emailError.classList.remove("hidden");
            isEmailValid = false;
            return;
        }

        try {
            const response = await fetch(`/Auth/CheckEmail?email=${encodeURIComponent(email)}`);
            const data = await response.json();

            if (data.exists) {
                emailError.textContent = "Email này đã được sử dụng.";
                emailError.classList.remove("hidden");
                isEmailValid = false;
            } else {
                emailError.classList.add("hidden");
                isEmailValid = true;
            }
        } catch (error) {
            console.error("Error checking email:", error);
            isEmailValid = true; // allow submit if check fails
        }
    });

    form.addEventListener("submit", function (e) {

        const email = document.getElementById("Email").value.trim();
        const password = passwordInput.value;
        const confirmPassword = document.getElementById("ConfirmPassword").value;
        const role = document.querySelector('input[name="Role"]:checked');

        if (!role) {
            alert("Vui lòng chọn vai trò");
            e.preventDefault();
            return;
        }

        if (!validateEmail(email)) {
            alert("Email không hợp lệ");
            e.preventDefault();
            return;
        }

        if (!isEmailValid) {
            e.preventDefault();
            return;
        }

        if (password !== confirmPassword) {
            alert("Password và Confirm Password không khớp");
            e.preventDefault();
            return;
        }

    });

});

function validateEmail(email) {

    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    return regex.test(email);

}