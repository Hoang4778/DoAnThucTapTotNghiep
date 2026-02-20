const selectHeaderTemplate = document.querySelector(".select-header-template");
const selectFooterTemplate = document.querySelector(".select-footer-template");

selectHeaderTemplate.onchange = () => {
  const inputHeaderTemplate = document.querySelector(
    "#header-template .ql-editor",
  );

  if (selectHeaderTemplate.value != "") {
    fetch(
      `/api/header-footer-template/get-one-template/${selectHeaderTemplate.value}`,
    )
      .then((response) => response.json())
      .then((result) => {
        if (!result.isOkay) {
          showPopup(result.message);
        }

        inputHeaderTemplate.innerHTML = result.data.content;
      });
  } else {
    inputHeaderTemplate.innerHTML = "";
  }
};

selectFooterTemplate.onchange = () => {
  const inputFooterTemplate = document.querySelector(
    "#footer-template .ql-editor",
  );

  if (selectFooterTemplate.value != "") {
    fetch(
      `/api/header-footer-template/get-one-template/${selectFooterTemplate.value}`,
    )
      .then((response) => response.json())
      .then((result) => {
        if (!result.isOkay) {
          showPopup(result.message);
        }

        inputFooterTemplate.innerHTML = result.data.content;
      });
  } else {
    inputFooterTemplate.innerHTML = "";
  }
};

const btnEditSetting = document.querySelector(".btn-edit-setting");
const loadingPart = document.querySelector(".loading-part");
const formWrapper = document.querySelector("form.wrapper");

btnEditSetting.onclick = (e) => {
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
  const selectHeaderTemplate = document.querySelector(
    ".select-header-template",
  );
  const selectFooterTemplate = document.querySelector(
    ".select-footer-template",
  );

  const userPUID = formWrapper.dataset.puid;
  const settingId = formWrapper.dataset.settingId;
  const languageTuple =
    inputLanguage.value != ""
      ? inputLanguage.value.split(" - ")
      : ["en", "English"];

  const setting = {
    id: parseInt(settingId),
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
    linkedHeaderTemplateCode:
      selectHeaderTemplate.value == null || selectHeaderTemplate.value == ""
        ? null
        : selectHeaderTemplate.value,
    linkedFooterTemplateCode:
      selectFooterTemplate.value == null || selectFooterTemplate.value == ""
        ? null
        : selectFooterTemplate.value,
  };

  fetch("/api/settings/header-footer/edit", {
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
