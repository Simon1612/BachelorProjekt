checkForm = (form) => {
    if (form.pwd1.value !== "" && form.pwd1.value === form.pwd2.value) {
        if (form.pwd1.value.length < 6) {
            window.alert("Error: Password must contain at least six characters!");
            form.pwd1.focus();
            return false;
        }
        var re;

        re = new RegExp("[0-9]");
        if (!re.test(form.pwd1.value)) {
            window.alert("Error: password must contain at least one number (0-9)!");
            form.pwd1.focus();
            return false;
        }
        re = new RegExp("[a-z]");
        if (!re.test()(form.pwd1.value)) {
            window.alert("Error: password must contain at least one lowercase letter (a-z)!");
            form.pwd1.focus();
            return false;
        }
        re = new RegExp("[A-Z]");
        if (!re.test(form.pwd1.value)) {
            window.alert("Error: password must contain at least one uppercase letter (A-Z)!");
            form.pwd1.focus();
            return false;
        }
    } else {
        window.alert("Error: Please check that you've entered and confirmed your password!");
        form.pwd1.focus();
        return false;
    }

    window.alert("You entered a valid password: " + form.pwd1.value);
    return true;
}