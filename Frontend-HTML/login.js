document.addEventListener("DOMContentLoaded", () => {
  const loginButton = document.getElementById("loginButton");
  const emailInput = document.getElementById("email");
  const senhaInput = document.getElementById("senha");
  const mensagemDiv = document.getElementById("mensagem");

  const API_URL = "http://localhost:5284/api/usuarios/login";

  loginButton.addEventListener("click", async () => {
    const email = emailInput.value;
    const senha = senhaInput.value;

    if (!email || !senha) {
      mensagemDiv.textContent = "Email e senha são obrigatórios.";
      return;
    }

    mensagemDiv.textContent = "";
    loginButton.textContent = "Aguarde...";
    loginButton.disabled = true;

    try {
      const response = await fetch(API_URL, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ email, senha }),
      });

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || "Erro ao tentar fazer login.");
      }

      localStorage.setItem("token", data.token);
      localStorage.setItem("user", JSON.stringify(data.usuario));

      window.location.href = "/index.html"; // Redireciona para a home
    } catch (error) {
      mensagemDiv.style.color = "red";
      mensagemDiv.textContent = error.message;
    } finally {
      loginButton.textContent = "Entrar";
      loginButton.disabled = false;
    }
  });
});
