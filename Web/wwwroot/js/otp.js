const inputs = document.querySelectorAll(".otp");
const codeInput = document.getElementById("Code");

if (inputs.length > 0 && codeInput) {

    inputs.forEach((input, index) => {

        // chỉ cho nhập số
        input.addEventListener("input", (e) => {

            input.value = input.value.replace(/[^0-9]/g, "");

            // chuyển sang ô tiếp theo
            if (input.value && index < inputs.length - 1) {
                inputs[index + 1].focus();
            }

            updateCode();
        });

        // xử lý backspace
        input.addEventListener("keydown", (e) => {

            if (e.key === "Backspace" && input.value === "" && index > 0) {
                inputs[index - 1].focus();
            }

        });

    });

    // paste OTP
    inputs[0].addEventListener("paste", (e) => {

        let paste = e.clipboardData.getData("text").trim();

        if (/^\d{6}$/.test(paste)) {

            paste.split("").forEach((num, i) => {
                if (inputs[i]) {
                    inputs[i].value = num;
                }
            });

            updateCode();
            inputs[5].focus();
        }

        e.preventDefault();
    });

}

// cập nhật hidden input Code
function updateCode() {

    let code = "";

    inputs.forEach(input => {
        code += input.value;
    });

    codeInput.value = code;

}

// =========================
// RESEND OTP
// =========================
const resendBtn = document.getElementById("resendCodeBtn");
const resendMsg = document.getElementById("resendMessage");
const emailInput = document.getElementById("Email");

if (resendBtn && emailInput) {
    resendBtn.addEventListener("click", async function (e) {
        e.preventDefault();

        // Prevent double click
        if (resendBtn.classList.contains("pointer-events-none")) return;

        const email = emailInput.value;
        if (!email) return;

        // UI Loading state
        resendBtn.textContent = "Đang gửi...";
        resendBtn.classList.add("pointer-events-none", "text-gray-400");
        resendBtn.classList.remove("text-primary", "hover:text-primary-hover", "hover:underline");
        resendMsg.classList.add("hidden");

        try {
            const response = await fetch('/Auth/ResendOtp', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: new URLSearchParams({ email: email })
            });

            const data = await response.json();

            resendMsg.textContent = data.message;
            resendMsg.classList.remove("hidden");

            if (data.success) {
                resendMsg.classList.remove("text-red-500");
                resendMsg.classList.add("text-green-600");

                // Cooldown 60s
                let timeLeft = 60;
                resendBtn.textContent = `Gửi lại sau ${timeLeft}s`;

                const timer = setInterval(() => {
                    timeLeft--;
                    resendBtn.textContent = `Gửi lại sau ${timeLeft}s`;

                    if (timeLeft <= 0) {
                        clearInterval(timer);
                        resetResendButton();
                    }
                }, 1000);

            } else {
                resendMsg.classList.remove("text-green-600");
                resendMsg.classList.add("text-red-500");
                resetResendButton();
            }

        } catch (error) {
            console.error("Error resending OTP:", error);
            resendMsg.textContent = "Đã có lỗi xảy ra. Vui lòng thử lại.";
            resendMsg.classList.remove("hidden", "text-green-600");
            resendMsg.classList.add("text-red-500");
            resetResendButton();
        }
    });

    function resetResendButton() {
        resendBtn.textContent = "Gửi lại mã";
        resendBtn.classList.remove("pointer-events-none", "text-gray-400");
        resendBtn.classList.add("font-semibold", "text-primary", "hover:text-primary-hover", "hover:underline");
    }
}