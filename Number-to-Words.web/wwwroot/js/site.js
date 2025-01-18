document.addEventListener("DOMContentLoaded", function () {
    const convertButton = document.getElementById("convertButton");
    const numberInput = document.getElementById("number-input");
    const resultTextElement = document.getElementById("resultText");
    const validationSummary = document.getElementById("validationSummary");
    const historyList = document.getElementById("historyList");
    const clearHistoryButton = document.getElementById("clearHistoryButton");
    const keypadButtons = document.querySelectorAll(".keypad-btn");
    const numberForm = document.getElementById("numberForm");

    // Define the maximum allowed number
    const MAX_NUMBER = 9999999999;

    // Event listener for the convert button
    convertButton.addEventListener("click", function (event) {
        event.preventDefault();
        submitForm();
    });

    // Event listeners for keypad buttons
    keypadButtons.forEach(button => {
        button.addEventListener("click", function () {
            const value = button.getAttribute("data-value");
            if (value === "C") {
                numberInput.value = "";
            } else {
                // Prevent multiple decimals
                if (value === "." && numberInput.value.includes(".")) {
                    return;
                }

                // Build the potential new value
                const newValue = numberInput.value + value;

                // Validate the new value
                if (isValidNumber(newValue)) {
                    numberInput.value = newValue;
                }
            }
            // Trigger input event for real-time validation
            numberInput.dispatchEvent(new Event('input'));
        });
    });

    // Event listener for clearing history
    clearHistoryButton.addEventListener("click", function () {
        historyList.innerHTML = "";
    });

    // Submit form when Enter key is pressed
    numberInput.addEventListener("keypress", function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
            submitForm();
        }
    });

    // Prevent input when number is too large
    numberInput.addEventListener("keydown", function (event) {
        const allowedKeys = ["Backspace", "Tab", "ArrowLeft", "ArrowRight", "Delete", "Enter", "Home", "End", "Escape"];
        if (allowedKeys.includes(event.key)) {
            return;
        }

        // If the key is not a number or a decimal point, prevent input
        if (!/[0-9.]/.test(event.key)) {
            event.preventDefault();
            return;
        }

        // Handle decimal point
        if (event.key === "." && numberInput.value.includes(".")) {
            event.preventDefault();
            return;
        }

        // Build the potential new value
        const currentValue = numberInput.value;
        const selectionStart = numberInput.selectionStart;
        const selectionEnd = numberInput.selectionEnd;
        const newValue = currentValue.slice(0, selectionStart) + event.key + currentValue.slice(selectionEnd);

        // Validate the new value
        if (!isValidNumber(newValue)) {
            event.preventDefault();
        }
    });

    // Prevent adding invalid numbers
    numberInput.addEventListener("paste", function (event) {
        const clipboardData = event.clipboardData || window.clipboardData;
        const pastedData = clipboardData.getData('Text');

        // Build the potential new value
        const currentValue = numberInput.value;
        const selectionStart = numberInput.selectionStart;
        const selectionEnd = numberInput.selectionEnd;
        const newValue = currentValue.slice(0, selectionStart) + pastedData + currentValue.slice(selectionEnd);

        // Validate the new value
        if (!isValidNumber(newValue)) {
            event.preventDefault();
        }
    });

    // Real-time validation
    numberInput.addEventListener("input", function () {
        validateInput();
    });

    // Handle form submission
    function submitForm() {
        if (!validateInput()) {
            return;
        }

        const number = numberInput.value.trim();

        const formData = new FormData();
        formData.append("NumberInput", number);

        fetch(numberForm.action, {
            method: "POST",
            body: formData
        })
            .then(response => {
                console.log("Response Status:", response.status);
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                try {
                    console.log("Response Data:", data);
                    if (data.success) {
                        resultTextElement.textContent = data.result;
                        addToHistory(number, data.result);
                        numberInput.value = "";
                        clearValidationError();
                        convertButton.disabled = true;
                    } else {
                        // Display validation errors from the server
                        displayValidationError(data.errors.join("\n"));
                    }
                } catch (error) {
                    console.error("Error in .then() block:", error);
                    alert(`An error occurred while processing the response: ${error.message}`);
                }
            })
            .catch(error => {
                console.error("Error in fetch operation:", error);
                alert(`An error occurred: ${error.message}`);
            });
    }

    // Validate input
    function validateInput() {
        const number = numberInput.value.trim();
        let isValid = true;

        if (number === "") {
            displayValidationError("Please enter a number.");
            isValid = false;
        } else if (isNaN(number)) {
            displayValidationError("Please enter a valid number.");
            isValid = false;
        } else {
            const numericValue = parseFloat(number);
            if (number.includes('-')) {
                displayValidationError(`Negative numbers are not allowed. Please enter a number between 0 and ${MAX_NUMBER}.`);
                isValid = false;
            } else if (numericValue < 0) {
                displayValidationError(`Negative numbers are not allowed. Please enter a number between 0 and ${MAX_NUMBER}.`);
                isValid = false;
            } else if (numericValue > MAX_NUMBER) {
                displayValidationError(`Number is too large. Please enter a number between 0 and ${MAX_NUMBER}.`);
                isValid = false;
            } else {
                clearValidationError();
            }
        }

        convertButton.disabled = !isValid;
        return isValid;
    }

    // Validate potential new input
    function isValidNumber(value) {
        // Allow empty value
        if (value === "") {
            return true;
        }

        // Check if value is a valid number
        const numericValue = parseFloat(value);

        if (isNaN(numericValue)) {
            return false;
        }

        // Disallow negative numbers
        if (numericValue < 0 || value.includes('-')) {
            return false;
        }

        // Disallow numbers exceeding the maximum allowed
        if (numericValue > MAX_NUMBER) {
            return false;
        }

        // Disallow multiple decimals
        if ((value.match(/\./g) || []).length > 1) {
            return false;
        }

        // Limit to two decimal places
        const decimalPart = value.split('.')[1];
        if (decimalPart && decimalPart.length > 2) {
            return false;
        }

        return true;
    }

    // Display validation errors
    function displayValidationError(message) {
        if (validationSummary) {
            validationSummary.textContent = message;
            validationSummary.style.display = "block";
        }

        // Add error class to the input
        numberInput.classList.add("is-invalid");
    }

    // Clear validation errors
    function clearValidationError() {
        if (validationSummary) {
            validationSummary.textContent = "";
            validationSummary.style.display = "none";
        }

        // Remove error class from the input
        numberInput.classList.remove("is-invalid");
    }

    // Add the result to the history list
    function addToHistory(number, words) {
        const historyItem = document.createElement("li");
        historyItem.className = "list-group-item";
        historyItem.textContent = `${number} -> ${words}`;
        historyList.appendChild(historyItem);
    }
});
