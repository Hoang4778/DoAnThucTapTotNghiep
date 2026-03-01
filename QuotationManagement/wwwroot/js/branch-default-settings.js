const loadingPart = document.querySelector(".loading-part");
const settingForm = document.querySelector(".settings-form");
const formVeil = document.querySelector(".form-veil");
const selectBranch = document.querySelector(".select-branch");
const quotationTypes = document.querySelectorAll(
  "input[name='quotation-type']",
);
const selectSetting = document.querySelector(".select-setting");
const hasGlobalDiscount = document.querySelector(
  ".checkbox-has-global-discount",
);
const globalDiscountAmount = document.querySelector(".input-global-discount");
const hasGlobalSurcharge = document.querySelector(
  ".checkbox-has-global-surcharge",
);
const globalSurchargeAmount = document.querySelector(".input-global-surcharge");
const defaultTaxRate = document.querySelector(".input-default-tax-rate");
const hasPageBreakBeforeTable = document.querySelector(
  ".checkbox-page-break-before-quotation-table",
);
const displayCustomerDetails = document.querySelector(
  ".checkbox-display-customer-details",
);
const displayQuantityAndPrice = document.querySelector(
  ".checkbox-display-quantity-and-unit-price",
);
const hasPageBreakAfterTable = document.querySelector(
  ".checkbox-page-break-after-quotation-table",
);
const showQuotationStartDate = document.querySelector(
  ".checkbox-quotation-start-date",
);
const showQuotationExpiryDate = document.querySelector(
  ".checkbox-quotation-expiration-date",
);
const signatureBoxes = document.querySelectorAll("input[name='signature-box']");
const quotationFooterPositions = document.querySelectorAll(
  "input[name='quotation-footer-position']",
);
const selectLetterTopTemplate = document.querySelector(
  ".select-letter-top-template",
);
const btnSaveSetting = document.querySelector(".btn-save");

selectBranch.onchange = () => {
  selectSetting.innerHTML = "<option value=''></option>";

  if (selectBranch.value != "") {
    fetch(
      `/api/branch-default-settings/get-setting-by-branch-code/${selectBranch.value}`,
    )
      .then((res) => res.json())
      .then((result) => {
        if (result.isOkay) {
          result.data.headerFooterSettings.forEach((setting) => {
            const option = document.createElement("option");
            option.textContent = setting.name;
            option.value = setting.code;
            selectSetting.appendChild(option);
          });

          const branchSetting = result.data.branchSetting;

          if (branchSetting.id) {
            settingForm.setAttribute("data-setting-id", branchSetting.id);
          }

          if (branchSetting.quotationTypeId) {
            quotationTypes.forEach((type) => {
              if (type.value == result.data.branchSetting.quotationTypeId) {
                type.checked = true;
              }
            });
          }

          if (branchSetting.defaultTaxRate != 0) {
            defaultTaxRate.value = branchSetting.defaultTaxRate;
          }

          if (branchSetting.headerFooterSettingCode != null) {
            selectSetting.value = branchSetting.headerFooterSettingCode;
          }

          if (branchSetting.hasGlobalDiscount) {
            hasGlobalDiscount.checked = true;
          }

          if (branchSetting.globalDiscountAmount != null) {
            globalDiscountAmount.value = branchSetting.globalDiscountAmount;
            globalDiscountAmount.disabled = false;
          } else {
            globalDiscountAmount.disabled = true;
          }

          if (branchSetting.hasGlobalSurcharge) {
            hasGlobalSurcharge.checked = true;
          }

          if (branchSetting.globalSurchargeAmount != null) {
            globalSurchargeAmount.value = branchSetting.globalSurchargeAmount;
            globalSurchargeAmount.disabled = false;
          } else {
            globalSurchargeAmount.disabled = true;
          }

          if (branchSetting.hasPageBreakBeforeTable) {
            hasPageBreakBeforeTable.checked = true;
          }

          if (branchSetting.hasContactInfoInCustomerDetails) {
            displayCustomerDetails.checked = true;
          }

          if (branchSetting.hasQuantityAndUnitPriceColumns) {
            displayQuantityAndPrice.checked = true;
          }

          if (branchSetting.hasPageBreakAfterTable) {
            hasPageBreakAfterTable.checked = true;
          }

          if (branchSetting.showQuotationStartDate) {
            showQuotationStartDate.checked = true;
          }

          if (branchSetting.showQuotationExpirationDate) {
            showQuotationExpiryDate.checked = true;
          }

          if (branchSetting.customerAcceptanceSignatureBoxId) {
            signatureBoxes.forEach((box) => {
              if (box.value == branchSetting.customerAcceptanceSignatureBoxId) {
                box.checked = true;
              }
            });
          }

          if (branchSetting.quotationFooterPositionId) {
            quotationFooterPositions.forEach((position) => {
              if (position.value == branchSetting.quotationFooterPositionId) {
                position.checked = true;
              }
            });
          }

          if (branchSetting.letterTopTemplateId) {
            selectLetterTopTemplate.value = branchSetting.letterTopTemplateId;
          }

          formVeil.classList.add("disabled");
        } else {
          showPopup(result.message);
          formVeil.classList.remove("disabled");
        }
      });
  } else {
    formVeil.classList.remove("disabled");
  }
};

hasGlobalDiscount.onchange = () => {
  if (hasGlobalDiscount.checked) {
    globalDiscountAmount.disabled = false;
  } else {
    globalDiscountAmount.disabled = true;
  }
};

hasGlobalSurcharge.onchange = () => {
  if (hasGlobalSurcharge.checked) {
    globalSurchargeAmount.disabled = false;
  } else {
    globalSurchargeAmount.disabled = true;
  }
};

btnSaveSetting.onclick = (e) => {
  e.preventDefault();
  loadingPart.classList.add("active");

  const settingForm = document.querySelector(".settings-form");
  const selectBranch = document.querySelector(".select-branch");
  const quotationType = document.querySelector(
    "input[name='quotation-type']:checked",
  );
  const defaultTaxRate = document.querySelector(".input-default-tax-rate");
  const selectSetting = document.querySelector(".select-setting");
  const hasGlobalDiscount = document.querySelector(
    ".checkbox-has-global-discount",
  );
  const globalDiscountAmount = document.querySelector(".input-global-discount");
  const hasGlobalSurcharge = document.querySelector(
    ".checkbox-has-global-surcharge",
  );
  const globalSurchargeAmount = document.querySelector(
    ".input-global-surcharge",
  );
  const hasPageBreakBeforeTable = document.querySelector(
    ".checkbox-page-break-before-quotation-table",
  );
  const displayCustomerDetails = document.querySelector(
    ".checkbox-display-customer-details",
  );
  const displayQuantityAndPrice = document.querySelector(
    ".checkbox-display-quantity-and-unit-price",
  );
  const hasPageBreakAfterTable = document.querySelector(
    ".checkbox-page-break-after-quotation-table",
  );
  const showQuotationStartDate = document.querySelector(
    ".checkbox-quotation-start-date",
  );
  const showQuotationExpiryDate = document.querySelector(
    ".checkbox-quotation-expiration-date",
  );
  const signatureBox = document.querySelector(
    "input[name='signature-box']:checked",
  );
  const quotationFooterPosition = document.querySelector(
    "input[name='quotation-footer-position']:checked",
  );
  const selectLetterTopTemplate = document.querySelector(
    ".select-letter-top-template",
  );

  if (selectBranch.value == "") {
    loadingPart.classList.remove("active");
    showPopup("The branch is not chosen. Please choose one branch.");
    return;
  }

  if (isNaN(parseFloat(defaultTaxRate.value))) {
    loadingPart.classList.remove("active");
    showPopup("The default tax rate is not a number. Please try again.");
    return;
  } else if (
    parseFloat(defaultTaxRate.value) > 100 ||
    parseFloat(defaultTaxRate.value) < 0
  ) {
    loadingPart.classList.remove("active");
    showPopup(
      "The default tax rate is not from 0% to 100%. Please put a suitable number.",
    );
    return;
  }

  if (hasGlobalDiscount.checked) {
    if (isNaN(parseFloat(globalDiscountAmount.value))) {
      loadingPart.classList.remove("active");
      showPopup("The global discount is not a number. Please try again.");
      return;
    } else if (
      parseFloat(globalDiscountAmount.value) > 100 ||
      parseFloat(globalDiscountAmount.value) < 0
    ) {
      loadingPart.classList.remove("active");
      showPopup(
        "The global discount is not from 0% to 100%. Please put a suitable number.",
      );
      return;
    }
  }

  if (hasGlobalSurcharge.checked) {
    if (isNaN(parseFloat(globalSurchargeAmount.value))) {
      loadingPart.classList.remove("active");
      showPopup("The global surcharge is not a number. Please try again.");
      return;
    } else if (
      parseFloat(globalSurchargeAmount.value) > 100 ||
      parseFloat(globalSurchargeAmount.value) < 0
    ) {
      loadingPart.classList.remove("active");
      showPopup(
        "The global surcharge is not from 0% to 100%. Please put a suitable number.",
      );
      return;
    }
  }

  const setting = {
    id:
      settingForm.dataset.settingId != undefined
        ? parseInt(settingForm.dataset.settingId)
        : null,
    branchCode: selectBranch.value,
    quotationTypeId:
      quotationType != null ? parseInt(quotationType.value) : null,
    defaultTaxRate: parseFloat(parseFloat(defaultTaxRate.value).toFixed(2)),
    headerFooterSettingCode:
      selectSetting.value != "" ? selectSetting.value : null,
    hasGlobalDiscount: hasGlobalDiscount.checked,
    globalDiscountAmount: hasGlobalDiscount.checked
      ? parseFloat(globalDiscountAmount.value)
      : null,
    hasGlobalSurcharge: hasGlobalSurcharge.checked,
    globalSurchargeAmount: hasGlobalSurcharge.checked
      ? parseFloat(globalSurchargeAmount.value)
      : null,
    hasPageBreakBeforeTable: hasPageBreakBeforeTable.checked,
    hasContactInfoInCustomerDetails: displayCustomerDetails.checked,
    hasQuantityAndUnitPriceColumns: displayQuantityAndPrice.checked,
    hasPageBreakAfterTable: hasPageBreakAfterTable.checked,
    showQuotationStartDate: showQuotationStartDate.checked,
    showQuotationExpirationDate: showQuotationExpiryDate.checked,
    customerAcceptanceSignatureBoxId:
      signatureBox != null ? parseInt(signatureBox.value) : null,
    quotationFooterPositionId:
      quotationFooterPosition != null
        ? parseInt(quotationFooterPosition.value)
        : null,
    letterTopTemplateId:
      selectLetterTopTemplate.value != ""
        ? parseInt(selectLetterTopTemplate.value)
        : null,
  };

  fetch("/api/branch-default-settings/create-or-update-setting", {
    method: "post",
    "Content-Type": "application/json",
    body: JSON.stringify(setting),
  })
    .then((res) => res.json())
    .then((result) => {
      loadingPart.classList.remove("active");
      showPopup(result.message);

      if (result.isOkay) {
        setTimeout(() => {
          window.location.href = "/settings/default";
        }, 3000);
      }
    });
};
