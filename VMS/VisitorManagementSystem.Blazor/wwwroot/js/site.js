window.showToast = (message) => {
    const container = document.getElementById("toast-container");

    const toast = document.createElement("div");
    toast.className = "toast-message";
    toast.innerText = message;

    container.appendChild(toast);

    setTimeout(() => {
        toast.classList.add("show");
    }, 100); // small delay for fade-in

    // auto remove after 5s
    setTimeout(() => {
        toast.classList.remove("show");
        setTimeout(() => toast.remove(), 500);
    }, 5000);
};
window.getElementPosition = (elementId) => {
    const el = document.getElementById(elementId);
    if (!el) return null;
    const rect = el.getBoundingClientRect();
    return {
        top: rect.top,
        left: rect.left,
        bottom: rect.bottom,
        right: rect.right,
        width: rect.width,
        height: rect.height
    };
};
