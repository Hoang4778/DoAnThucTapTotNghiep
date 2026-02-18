function showPopup(message) {
  document.getElementById("popup-message").textContent = message;
  document.getElementById("popup").classList.remove("hidden");
}

document.getElementById("popup-close").addEventListener("click", () => {
  document.getElementById("popup").classList.add("hidden");
});
