
function HideShowElement(selector, hideShow)
{
	if (hideShow == HideShow.HIDE)
	{
		$(selector).addClass("hidden");
	}
	else if (hideShow == HideShow.SHOW)
	{
		$(selector).removeClass("hidden");
	}
}

function HideShowElementVisibity(selector, hideShow)
{
	if (hideShow == HideShow.HIDE)
	{
		$(selector).addClass("invisible");
	}
	else if (hideShow == HideShow.SHOW)
	{
		$(selector).removeClass("invisible");
	}
}

function StringNullOrEmpty(value)
{
	return value == null || value.trim().length == 0;
}

function IsNotEmptyString(value)
{
	return value != null && value.length > 0;
}

function EnsureJQueryId(Id)
{
	if (IsNotEmptyString(Id) && Id[0] == "#")
		return Id;

	Id = (IsNotEmptyString(Id) && Id[0] != "#") ? `#${Id}` : "invalid";

	return Id;
}

function IsEmptyInput(textBoxId)
{
	textBoxId = EnsureJQueryId(textBoxId);

	var value = $(textBoxId).val();

	return StringNullOrEmpty(value);
}

function IsEmptyText(textBoxId)
{
	textBoxId = EnsureJQueryId(textBoxId);

	var value = $(textBoxId).text();

	return StringNullOrEmpty(value);
}

function CheckEmail(email)
{
	if (email == null || email == "")
		return false;

	var emailRegex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;

	return email.match(emailRegex);
}

function CheckInputEmail(emailInputId)
{
	emailInputId = EnsureJQueryId(emailInputId);

	var email = $(emailInputId).val();

	return CheckEmail(email);
}

function IsCheckboxChecked(checkboxId)
{
	checkboxId = EnsureJQueryId(checkboxId);

	return $(checkboxId).is(':checked');
}

function CheckOrUncheckInput(checkboxId, check)
{
	checkboxId = EnsureJQueryId(checkboxId);
	$(checkboxId).prop("checked", check);
}

function SelectAnOptionWithValue(selectListId, searchValue)
{
	selectListId = EnsureJQueryId(selectListId);

	$(`${selectListId} option[value='${searchValue}']`).attr("selected", true);
}

function RadioGroupHasAtLeastOneChecked(radioGroupName)
{
	return $(`input:radio[name="${radioGroupName}"]:checked`).length > 0;
}

function GetIdOfSelectedRadioInGroup(radioGroupName)
{
	return $(`input[name="${radioGroupName}"]:checked`).attr('id');
}

function GetTextOfSelectedOptionInSelectList(id)
{
	id = EnsureJQueryId(id);

	return $(`${id} option:selected`).text();
}

function CheckPasswordStrength()
{
	$("#PasswordError").addClass("d-none"); // Hide error message

	var password = $("#psw-input").val();
	var strengthBar = $("#passwordStrengthBar");
	var minLength = $("#minLength");
	var numberCheck = $("#numberCheck");
	var uppercaseCheck = $("#uppercaseCheck");
	var lowercaseCheck = $("#lowercaseCheck");
	var specialCharCheck = $("#specialCharCheck");

	var passwordStrength = 0;

	// Criteria checks
	if (password.length >= 8)
	{
		minLength.addClass("text-success").removeClass("text-muted");
		passwordStrength++;
	}
	else
	{
		minLength.addClass("text-muted").removeClass("text-success");
	}

	if (/[0-9]/.test(password))
	{
		numberCheck.addClass("text-success").removeClass("text-muted");
		passwordStrength++;
	}
	else
	{
		numberCheck.addClass("text-muted").removeClass("text-success");
	}

	if (/[A-Z]/.test(password))
	{
		uppercaseCheck.addClass("text-success").removeClass("text-muted");
		passwordStrength++;
	}
	else
	{
		uppercaseCheck.addClass("text-muted").removeClass("text-success");
	}

	if (/[a-z]/.test(password))
	{
		lowercaseCheck.addClass("text-success").removeClass("text-muted");
		passwordStrength++;
	}
	else
	{
		lowercaseCheck.addClass("text-muted").removeClass("text-success");
	}

	if (/[^A-Za-z0-9]/.test(password))
	{
		specialCharCheck.addClass("text-success").removeClass("text-muted");
		passwordStrength++;
	}
	else
	{
		specialCharCheck.addClass("text-muted").removeClass("text-success");
	}

	// Update the progress bar
	var width = passwordStrength * 20; // Each criterion contributes 20%

	strengthBar.css("width", width + "%").attr("aria-valuenow", width);

	if (passwordStrength === 0)
	{
		strengthBar.removeClass("bg-success bg-warning").addClass("bg-danger");
	}
	else if (passwordStrength <= 2)
	{
		strengthBar.removeClass("bg-success bg-warning").addClass("bg-danger");
	}
	else if (passwordStrength === 3)
	{
		strengthBar.removeClass("bg-success bg-danger").addClass("bg-warning");
	}
	else if (passwordStrength === 4)
	{
		strengthBar.removeClass("bg-danger").addClass("bg-warning");
	}
	else
	{
		strengthBar.removeClass("bg-danger bg-warning").addClass("bg-custom-success");
	}
	return passwordStrength == 5;
}

function ValidatePhoneNumber()
{
	const phoneInput = $("#PhoneNumber");
	const countryCode = $("#countryCode").val();
	const fullPhoneNumber = countryCode + phoneInput.val().replace(/\s+/g, ""); // Remove spaces for validation

	const phoneNumberPattern = /^[0-9]{8,14}$/; // Example: Phone number must be 9-14 digits
	const isValid = phoneNumberPattern.test(phoneInput.val());

	if (!isValid)
	{
		$("#PhoneNumberError").removeClass("d-none").text("Invalid phone number format");
		phoneInput.addClass("is-invalid");
	} 
	else
	{
		$("#PhoneNumberError").addClass("d-none");
		phoneInput.removeClass("is-invalid").addClass("is-valid");
	}
	return isValid;
}

function FormatToDecimal(value, decimals = 2) 
{
    const num = parseFloat(value);

    if (isNaN(num)) return "0.00"; 

    return num.toFixed(decimals);
}