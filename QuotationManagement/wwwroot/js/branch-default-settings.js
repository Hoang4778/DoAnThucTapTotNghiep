const selectBranch = document.querySelector(".select-branch");
const formVeil = document.querySelector(".form-veil");

selectBranch.onchange = () => {
  if (selectBranch.value != "") {
    fetch(
      `/api/branch-default-settings/get-setting-by-branch-code/${selectBranch.value}`,
    )
      .then((res) => res.json())
      .then((result) => {
        if (result.isOkay) {
          console.log(result.data);
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
