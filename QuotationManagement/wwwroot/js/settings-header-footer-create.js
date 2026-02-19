const selectHeaderTemplate = document.querySelector(".select-header-template");
const selectFooterTemplate = document.querySelector(".select-footer-template");

selectHeaderTemplate.onchange = () => {
  const inputHeaderTemplate = document.querySelector(
    "#header-template .ql-editor",
  );

  if (selectHeaderTemplate.value != "") {
  }
};

selectFooterTemplate.onchange = () => {
  const inputFooterTemplate = document.querySelector(
    "#footer-template .ql-editor",
  );

  if (selectFooterTemplate.value != "") {
  }
};

const btnCreateSetting = document.querySelector(".btn-create-setting");
const loadingPart = document.querySelector(".loading-part");
const formWrapper = document.querySelector("form.wrapper");

btnCreateSetting.onclick = (e) => {
  e.preventDefault();
  loadingPart.classList.add("active");

  const inputSettingName = document.querySelector(".input-setting-name");
  const inputSettingStatus = document.querySelector(
    "input[name='setting-status']:checked",
  );
  const inputBranch = document.querySelector(".select-branch");
  const inputLanguage = document.querySelector(".select-language");
  const inputHeaderTemplate = document.querySelector(
    "#header-template .ql-editor",
  );
  const inputFooterTemplate = document.querySelector(
    "#footer-template .ql-editor",
  );

  const userPUID = formWrapper.dataset.puid;
  const languageTuple =
    inputLanguage.value != ""
      ? inputLanguage.value.split(" - ")
      : ["en", "English"];

  const setting = {
    code: userPUID,
    name:
      inputSettingName.value != ""
        ? inputSettingName.value.trim()
        : "New setting",
    branchCode: inputBranch.value != "" ? inputBranch.value : null,
    languageName: languageTuple[1],
    languageCode: languageTuple[0],
    status:
      inputSettingStatus != null
        ? inputSettingStatus.value == "true"
          ? true
          : false
        : true,
    headerTemplate: inputHeaderTemplate.innerHTML.trim(),
    footerTemplate: inputFooterTemplate.innerHTML.trim(),
    createdOn: null,
    updatedOn: null,
  };

  fetch("/api/settings/header-footer/create", {
    method: "post",
    "Content-Type": "application/json",
    body: JSON.stringify(setting),
  })
    .then((response) => response.json())
    .then((result) => {
      loadingPart.classList.remove("active");
      showPopup(result.message);

      if (result.isOkay) {
        setTimeout(() => {
          window.location.href = "/settings/header-footer";
        }, 3000);
      }
    });
};
