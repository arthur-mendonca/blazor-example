document.addEventListener("DOMContentLoaded", () => {
  // Verifica se o usuário está logado, caso contrário, redireciona para o login
  const token = localStorage.getItem("token");
  if (!token) {
    window.location.href = "/login.html";
    return; // Interrompe a execução do script se não estiver logado
  }

  initializeCarousel();
});

function initializeCarousel() {
  const images = [
    "./img/cel1.webp",
    "./img/cel2.webp",
    "./img/cel3.webp",
    "./img/cel4.webp",
    "./img/cel5.webp",
    "./img/cel6.webp",
  ];
  let currentIndex = 0;

  const carouselInner = document.getElementById("carousel-inner");
  const prevBtn = document.getElementById("prev-btn");
  const nextBtn = document.getElementById("next-btn");
  const indicatorsContainer = document.getElementById("carousel-indicators");

  // Limpa conteúdo anterior para evitar duplicação
  carouselInner.innerHTML = "";
  indicatorsContainer.innerHTML = "";

  images.forEach((src, index) => {
    const imgContainer = document.createElement("div");
    imgContainer.className =
      "absolute w-full h-full transition-opacity duration-700 ease-in-out";
    imgContainer.style.opacity = index === 0 ? "1" : "0";

    const img = document.createElement("img");
    img.src = src;
    img.className = "object-contain w-full h-full mx-auto";
    imgContainer.appendChild(img);
    carouselInner.appendChild(imgContainer);

    const button = document.createElement("button");
    button.className = `w-3 h-3 rounded-full ${
      index === 0 ? "bg-white" : "bg-gray-400"
    }`;
    button.addEventListener("click", () => goToIndex(index));
    indicatorsContainer.appendChild(button);
  });

  const carouselItems = carouselInner.querySelectorAll("div");
  const indicatorButtons = indicatorsContainer.querySelectorAll("button");

  function updateCarousel() {
    carouselItems.forEach((item, index) => {
      item.style.opacity = index === currentIndex ? "1" : "0";
    });
    indicatorButtons.forEach((button, index) => {
      button.className = `w-3 h-3 rounded-full ${
        index === currentIndex ? "bg-white" : "bg-gray-400"
      }`;
    });
  }

  function nextImage() {
    currentIndex = (currentIndex + 1) % images.length;
    updateCarousel();
  }

  function prevImage() {
    currentIndex = (currentIndex - 1 + images.length) % images.length;
    updateCarousel();
  }

  function goToIndex(index) {
    currentIndex = index;
    updateCarousel();
  }

  nextBtn.addEventListener("click", nextImage);
  prevBtn.addEventListener("click", prevImage);

  setInterval(nextImage, 5000); // Auto-play
}
