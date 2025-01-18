# **Test Plan for Number-to-Words Converter Application**

---

## **1. Introduction**

### **1.1 Scope**

This Test Plan outlines the strategy for testing the **Number-to-Words Converter** application. The application converts numeric currency amounts into their corresponding English words, handling both dollars and cents. The testing ensures that the application functions correctly, and provides accurate conversions.

#### **1.1.1 In Scope**

- **Unit Testing:**
  - Testing the core conversion logic within the `EnglishNumberToWordsConverter` and `NumberToWordsService` classes.
  - Validating data annotations and server-side validation in the `NumberToWordsViewModel`.

- **Interactive Testing:**
  - Manually testing the web interface to verify user input handling, validation messages, and result display.
  - Ensuring the keypad functionality works as intended (e.g., number entry, decimal handling, clear operation).

#### **1.1.2 Out of Scope**

- **Security Testing:**
  - Comprehensive security assessments (e.g., penetration testing) are not included.

- **Performance Testing:**
  - Detailed performance benchmarking beyond basic responsiveness.

- **Localisation:**
  - Support for languages other than English is not covered.

### **1.2 Quality Objectives**

- **Accuracy:** Ensure that all numeric inputs are correctly converted into their corresponding English words, accurately handling both dollars and cents.

- **Reliability:** The application should perform consistently across various input scenarios without failures or unexpected behaviours.

- **Usability:** Provide a user-friendly interface with clear validation messages and intuitive interaction mechanisms.

- **Maintainability:** Ensure that the codebase remains organised and testable, facilitating future enhancements.

## **2. Test Methodology**

### **2.1 Overview**

The testing approach combines **unit testing** for backend logic and **manual interactive testing** for the web interface. Unit tests are automated using the **xUnit** framework with **Moq** for mocking dependencies. Manual testing involves interacting with the web application through a browser to validate user experience and input handling.

### **2.2 Test Levels**

1. **Unit Testing:**
   - **Objective:** Verify individual components (methods/classes) function as intended.
   - **Components Tested:**
     - `EnglishNumberToWordsConverter` class.
     - `NumberToWordsService` class.
     - `NumberToWordsViewModel` data annotations and validation.

2. **Interactive Testing:**
   - **Objective:** Validate the application’s functionality through the web interface.
   - **Focus Areas:**
     - User input handling and validation.
     - Accurate conversion and result display.
     - Keypad functionality (number entry, decimal handling, clear operation).

## **3. Test Deliverables**

- **Test Plan Document:** This document outlining the testing strategy and approach.
- **Unit Test Cases:** Automated test cases written using xUnit and Moq.
- **Manual Test Cases:** Documented scenarios for interactive testing via the web interface.
- **Test Results:** Records of test executions, including passed and failed tests.
- **Defect Reports:** Documentation of any identified issues during testing.
- **Final Test Summary:** An overview of testing activities and outcomes.

## **4. Resource & Environment Needs**

### **4.1 Testing Tools**

- **Unit Testing Framework:** xUnit
- **Mocking Framework:** Moq
- **Integrated Development Environment (IDE):** Visual Studio 2022 or later
- **Browser for Interactive Testing:** Latest versions of Chrome, Firefox, or Safari

### **4.2 Test Environment**

- **Hardware Requirements:**
  - A development machine with adequate processing power and memory to run Visual Studio and testing tools efficiently.

- **Configuration:**
  - Ensure the application is correctly deployed and running locally for interactive testing.

---

## **5. Detailed Test Cases**

### **5.1 Unit Testing**

#### **5.1.1 EnglishNumberToWordsConverterTests**

| **Test Case ID** | **Description**                        | **Input** | **Expected Output**                        |
|-------------------|----------------------------------------|-----------|--------------------------------------------|
| TC-EN-01          | Convert Zero                           | `0`       | `"ZERO"`                                   |
| TC-EN-02          | Convert Single Digit                   | `1`       | `"ONE"`                                    |
| TC-EN-03          | Convert Teen Number                    | `15`      | `"FIFTEEN"`                                |
| TC-EN-04          | Convert Double Digit                   | `23`      | `"TWENTY-THREE"`                           |
| TC-EN-05          | Convert Large Double Digit             | `85`      | `"EIGHTY-FIVE"`                            |
| TC-EN-06          | Convert Hundred                        | `100`     | `"ONE HUNDRED AND ZERO"`                    |
| TC-EN-07          | Convert Three Digits                   | `123`     | `"ONE HUNDRED AND TWENTY-THREE"`           |
| TC-EN-08          | Convert Thousand                       | `1000`    | `"ONE THOUSAND AND ZERO"`                   |
| TC-EN-09          | Convert Four Digits                    | `1234`    | `"ONE THOUSAND TWO HUNDRED AND THIRTY-FOUR"`|
| TC-EN-10          | Convert Million                        | `1000000` | `"ONE MILLION AND ZERO"`                    |
| TC-EN-11          | Convert Billion                        | `1000000000` | `"ONE BILLION AND ZERO"`                |

#### **5.1.2 NumberToWordsServiceTests**

| **Test Case ID** | **Description**                          | **Input** | **Expected Output**                                          |
|-------------------|------------------------------------------|-----------|--------------------------------------------------------------|
| TC-NTW-01         | Convert Zero Amount                      | `0.00`    | `"ZERO DOLLARS AND ZERO CENTS"`                              |
| TC-NTW-02         | Convert Positive Amount with Cents       | `123.45`  | `"ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS"` |
| TC-NTW-03         | Convert Amount with Zero Cents           | `5.00`    | `"FIVE DOLLARS AND ZERO CENTS"`                              |
| TC-NTW-04         | Convert Amount with Zero Dollars         | `0.75`    | `"ZERO DOLLARS AND SEVENTY-FIVE CENTS"`                      |
| TC-NTW-05         | Convert Singular Dollar and Cent         | `1.01`    | `"ONE DOLLAR AND ONE CENT"`                                  |
| TC-NTW-06         | Convert Plural Dollars and Cents         | `2.02`    | `"TWO DOLLARS AND TWO CENTS"`                                |

### **5.2 Interactive Testing via Web Interface**

| **Test Case ID** | **Description**                          | **Input**          | **Expected Outcome**                                         |
|-------------------|------------------------------------------|--------------------|--------------------------------------------------------------|
| TC-WEB-01         | Enter Zero                               | `0`                | Displays `"ZERO DOLLARS AND ZERO CENTS"`                     |
| TC-WEB-02         | Enter Maximum Allowed Number            | `9999999999`       | Displays `"NINE BILLION NINE HUNDRED AND NINETY-NINE MILLION NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS AND ZERO CENTS"`                  |
| TC-WEB-03         | Enter Valid Amount with Cents            | `1234.56`          | Displays `"ONE THOUSAND TWO HUNDRED AND THIRTY-FOUR DOLLARS AND FIFTY-SIX CENTS"` |
| TC-WEB-04         | Enter Amount with Zero Cents             | `5.00`             | Displays `"FIVE DOLLARS AND ZERO CENTS"`                     |
| TC-WEB-08         | Submit Empty Input                       | *(Empty)*          | Displays validation error: `"Please enter a number."`        |
| TC-WEB-09         | Enter More Than Two Decimal Places        | `123.456`          | Prevents input beyond two decimal places and/or displays appropriate validation error |
| TC-WEB-10         | Exceed Maximum Input Length via Keypad    | *(Input exceeding max length)* | Prevents additional input beyond allowed length          |
| TC-WEB-11         | Use Keypad to Enter and Clear Input        | *(Use on-screen keypad)* | Numbers are entered correctly, clear button works as intended |

---

## **6. Execution Steps**

### **6.1 Unit Testing**

1. **Setup:**
   - Ensure that the `Number-to-Words.tests` project is included in the solution.
   - Verify that all necessary NuGet packages (`xUnit`, `Moq`) are installed.

2. **Run Tests:**
   - Open **Test Explorer** in Visual Studio (`Test`).
   - Click **Run All Tests** to execute all unit tests.
   - Observe the results, ensuring that all tests pass successfully.

3. **Analyse Failures:**
   - If any tests fail, review the error messages.
   - Debug and fix the corresponding code in the application.
   - Re-run tests to confirm that fixes resolve the issues.

### **6.2 Interactive Testing via Web Interface**

1. **Launch Application:**
   - Start the web application
   - Open a web browser and navigate to the application's URL.

2. **Execute Test Cases:**
   - **Valid Inputs:**
     - Enter numbers using the input field or keypad.
     - Verify that the displayed words match the expected outputs.
   
   - **Invalid Inputs:**
     - Confirm that appropriate validation messages are displayed and that invalid inputs are not processed.
   
   - **Edge Cases:**
     - Leave the input field empty and attempt to submit.
     - Use the keypad to enter numbers and clear them, ensuring that validation messages reset correctly.

---

# **Conclusion**

This Test Plan provides a structured approach to validating the Number-to-Words Converter application, ensuring that both backend logic and the web interface function correctly and meet quality standards. By following this plan, you can systematically verify that the application accurately converts numbers to words, handles user inputs effectively, and offers a reliable and user-friendly experience.

