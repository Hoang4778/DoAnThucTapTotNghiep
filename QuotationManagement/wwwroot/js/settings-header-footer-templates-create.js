const btnCreateTemplate = document.querySelector(".btn-create-template");
const loadingPart = document.querySelector(".loading-part");
const formWrapper = document.querySelector("form.wrapper");

btnCreateTemplate.onclick = (e) => {
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

  const userPUID = formWrapper.dataset.puid;
  const languageTuple =
    inputLanguage.value != ""
      ? inputLanguage.value.split(" - ")
      : ["en", "English"];

  const template = {
    code: userPUID,
    name:
      inputTemplateName.value != ""
        ? inputTemplateName.value.trim()
        : "New template",
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
    createdOn: null,
    updatedOn: null,
  };

  console.log(template);
};
