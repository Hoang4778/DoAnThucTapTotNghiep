const btnSearchQuotation = document.querySelector(".btn-search-quotation");
const loadingPart = document.querySelector(".loading-part");
const quotationListTable = document.querySelector(".quotation-list");

btnSearchQuotation.onclick = (e) => {
  e.preventDefault();
  loadingPart.classList.add("active");

  const quotationCode = document.querySelector(".input-quotation-code");
  const quotationName = document.querySelector(".input-quotation-name");
  const customerName = document.querySelector(".input-customer-name");
  const accountCode = document.querySelector(".input-customer-name");
  const creationDateFrom = document.querySelector(".input-creation-date-from");
  const creationDateTo = document.querySelector(".input-creation-date-to");
  const expirationDateFrom = document.querySelector(
    ".input-expiration-date-from",
  );
  const expirationDateTo = document.querySelector(".input-expiration-date-to");
  const statusInProgress = document.querySelector(
    ".checkbox-status-in-progress",
  );
  const statusValidated = document.querySelector(".checkbox-status-validated");
  const statusExpired = document.querySelector(".checkbox-status-expired");
  const statusCancelled = document.querySelector(".checkbox-status-cancelled");

  const query = {
    quotationCode: quotationCode.value,
    quotationName: quotationName.value,
    customerName: customerName.value,
    accountCode: accountCode.value,
    creationDateFrom: creationDateFrom.value,
    creationDateTo: creationDateTo.value,
    expirationDateFrom: expirationDateFrom.value,
    expirationDateTo: expirationDateTo.value,
    statusInProgress: statusInProgress.value,
    statusValidated: statusValidated.value,
    statusExpired: statusExpired.value,
    statusCancelled: statusCancelled.value,
  };

  const queryList = [];
  for (const key in query) {
    queryList.push(`${key}=${query[key]}`);
  }

  const queryStr = queryList.join("&");

  fetch(`/api/quotation/search?${queryStr}`)
    .then((res) => res.json())
    .then((result) => {
      loadingPart.classList.remove("active");

      if (result.isOkay) {
        quotationListTable.innerHTML = "";

        result.data.forEach((q) => {
          const row = document.createElement("tr");

          for (const attr in q) {
            const cell = document.createElement("td");
            cell.textContent = q[attr];
            row.appendChild(cell);
          }

          quotationListTable.appendChild(row);
        });
      } else {
        showPopup(result.message);
      }
    });
};
