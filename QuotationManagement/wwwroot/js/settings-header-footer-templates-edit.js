const btnEditTemplate = document.querySelector(".btn-edit-template");
const loadingPart = document.querySelector(".loading-part");
const formWrapper = document.querySelector("form.wrapper");

btnEditTemplate.onclick = (e) => {
  e.preventDefault();
  loadingPart.classList.add("active");

  const inputTemplateName = document.querySelector(".input-template-name");
  const inputTemplateStatus = document.querySelector(
    "input[name='template-status']:checked",
  );
  const inputBranch = document.querySelector(".select-branch");
  const inputLanguage = document.querySelector(".select-language");
  const inputUsableAsHeader = document.querySelector(
    "input[name='usableAsHeader']",
  );
  const inputUsableAsFooter = document.querySelector(
    "input[name='usableAsFooter']",
  );
  const inputContent = document.querySelector(
    ".section-template-content-content #editor .ql-editor",
  );

  const templateId = formWrapper.dataset.templateId;
  const templateCode = formWrapper.dataset.templateCode;
  const languageTuple =
    inputLanguage.value != ""
      ? inputLanguage.value.split(" - ")
      : ["en", "English"];

  const template = {
    id: parseInt(templateId),
    code: templateCode,
    name:
      inputTemplateName.value != ""
        ? inputTemplateName.value.trim()
        : "Edited template",
    branchCode: inputBranch.value != "" ? inputBranch.value : null,
    languageName: languageTuple[1],
    languageCode: languageTuple[0],
    usableAsHeader: inputUsableAsHeader.checked,
    usableAsFooter: inputUsableAsFooter.checked,
    status:
      inputTemplateStatus != null
        ? inputTemplateStatus.value == "true"
          ? true
          : false
        : true,
    content: inputContent.innerHTML.trim(),
    updatedOn: null,
  };

  fetch("/api/settings/header-footer-template/edit", {
    method: "post",
    "Content-Type": "application/json",
    body: JSON.stringify(template),
  })
    .then((response) => response.json())
    .then((result) => {
      loadingPart.classList.remove("active");
      showPopup(result.message);

      if (result.isOkay) {
        setTimeout(() => {
          window.location.href = "/settings/header-footer/templates";
        }, 3000);
      }
    });
};
