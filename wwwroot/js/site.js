document.addEventListener("DOMContentLoaded", () => {
    const authButton = document.getElementById("auth-button");
    if (authButton) {
        authButton.addEventListener('click', authButtonClick);
    }
    else {
        console.error("Element not found: auth-button");
    }
});
document.addEventListener("DOMContentLoaded", () => {
    const logoutButton = document.getElementById("logout-button");
    if (logoutButton) {
        logoutButton.addEventListener('click', logoutButtonClick);
    }
    else {
        console.error("Element not found: logout-button");
    }
});


function authButtonClick() {
    const authLogin = document.getElementById("auth-login");
    if (!authLogin) throw "Element not found: auth-login";
    const authPassword = document.getElementById("auth-password");
    if (!authPassword) throw "Element not found: auth-password";
    if (authLogin.value.length === 0) {
        alert("Необхiдно ввести логiн");
        return;
    }
    if (authPassword.value.length === 0) {
        alert("Необхiдно ввести пароль");
        return;
    }
    window
        .fetch(
            "/User/Auth", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    login: authLogin.value,
                    password: authPassword.value
                })
            }
        )
        .then(r => r.json())
        .then(j => {
            console.log(j);
            if (j.success) {
                // Перезагрузка страницы при успешной аутентификации
                location.reload();
            } else {
                // Вывод сообщения об отказе
                alert("Аутентификация не удалась. Пожалуйста, проверьте логин и пароль.");
            }
        });
}
function logoutButtonClick() {
    fetch("/User/Logout", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data.Success) {
                location.reload();
            } else {
                alert("Failed to logout. Please try again.");
            }
        })
        .catch(error => {
            console.log(error);
        });
}