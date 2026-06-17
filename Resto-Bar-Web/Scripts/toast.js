function mostrarErrorYRedirigir(mensaje, url) {

    const toast = document.getElementById("toastError");
    const texto = document.getElementById("toastMensaje");

    texto.innerText = mensaje;

    toast.classList.add("show");

    setTimeout(function () {
        window.location.href = url;
    }, 3000);
}