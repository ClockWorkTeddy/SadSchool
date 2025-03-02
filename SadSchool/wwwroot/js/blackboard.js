// Wait for DOM to load
document.addEventListener('DOMContentLoaded', () => {
    const canvas = document.getElementById('drawingCanvas');
    const ctx = canvas.getContext('2d');
    let isDrawing = false;
    let lastX = 0;
    let lastY = 0;

    // Initialize SignalR connection
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/blackboardHub")
        .build();

    // Event Listeners
    canvas.addEventListener('mousedown', startDrawing);
    canvas.addEventListener('mousemove', draw);
    canvas.addEventListener('mouseup', endDrawing);
    canvas.addEventListener('mouseout', endDrawing);

    document.getElementById('clearButton').addEventListener('click', () => {
        connection.invoke("ClearBoard");
    });

    // SignalR Methods
    connection.on("ReceiveDrawingData", (prevX, prevY, currentX, currentY, color, lineWidth) => {
        drawOnCanvas(prevX, prevY, currentX, currentY, color, lineWidth);
    });

    connection.on("ClearBoard", () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    });

    // Start connection
    connection.start()
        .then(() => console.log("SignalR Connected"))
        .catch(err => console.error("SignalR Connection Error: ", err));

    // Drawing functions
    function startDrawing(e) {
        isDrawing = true;
        [lastX, lastY] = [e.offsetX, e.offsetY];
    }

    // Clear button
    document.getElementById('clearButton').addEventListener('click', () => {
        connection.invoke("ClearBoard");
    });


    function draw(e) {
        if (!isDrawing) return;

        const currentX = e.offsetX;
        const currentY = e.offsetY;
        const colorSelector = document.getElementById("colorSelector");
        const color = colorSelector.value; // Default color
        const lineWidth = 3;

        // Draw locally
        drawOnCanvas(lastX, lastY, currentX, currentY, color, lineWidth);

        // Send to server
        connection.invoke("SendDrawingData", lastX, lastY, currentX, currentY, color, lineWidth);

        [lastX, lastY] = [currentX, currentY];
    }

    function endDrawing() {
        isDrawing = false;
    }

    function drawOnCanvas(prevX, prevY, currentX, currentY, color, lineWidth) {
        ctx.beginPath();
        ctx.moveTo(prevX, prevY);
        ctx.lineTo(currentX, currentY);
        ctx.strokeStyle = color;
        ctx.lineWidth = lineWidth;
        ctx.stroke();
        ctx.closePath();
    }

    const selectElement = document.getElementById("colorSelector");

    // Function to change the background color based on selected value
    function changeColor() {
        const count = selectElement.options.length;
        for (i = 0; i < count; i++) {
            selectElement.options[i].style.color = selectElement.options[i].value;
        }

        selectElement.style.color = selectElement.options[selectElement.selectedIndex].value;

        selectElement.addEventListener('mouseover', function () {
            selectElement.style.borderColor = selectElement.style.color; // Change color on hover
        });

        selectElement.addEventListener('mouseout', function () {
            selectElement.style.borderColor = '#70717d'; // Change color on hover
        });

    }

    // Set initial color based on default selection
    changeColor();

    // Add event listener for change event
    selectElement.addEventListener("change", changeColor);
});