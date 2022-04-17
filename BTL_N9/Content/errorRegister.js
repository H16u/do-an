function kiemTraUserName() {
    const un = document.getElementById("tenDangNhap");
    var e = ".";
    if (un.value === "" || un.value == null) {
        e = "Tên đăng nhập không được để trống!";
    } else {
        //var mailformat = /^[a-z0-9_]{6,50}$/;
        //if (un.value.match(mailformat)) {

        //} else {
        //    e = "Tên đăng nhập không hợp lệ!";
        }
    }
    document.getElementById("errorUserName").innerHTML = e;
}

function kiemTraPassWord() {
    const un = document.getElementById("matKhau");
    var e = ".";
    if (un.value === "" || un.value == null) {
        e = "Mật khẩu không được để trống!";
    } else {
        //var mailformat = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,50}$/;
        //if (un.value.match(mailformat)) {

        //} else {
        //    e = "Mật khẩu không hợp lệ!";
        }
    }
    document.getElementById("errorPassWord").innerHTML = e;
}

function kiemTraHoTen() {
    const un = document.getElementById("hoTen");
    var e = ".";
    if (un.value === "" || un.value == null) {
        e = "Họ tên không được để trống!";
    } else {
        //var mailformat = /^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)/;
        //if (un.value.match(mailformat)) {

        //} else {
        //    e = "Họ tên không hợp lệ!";
        }
    }
    document.getElementById("errorHoTen").innerHTML = e;
}

function kiemTraDiaChi() {
    const un = document.getElementById("diaChi");
    var e = ".";
    if (un.value === "" || un.value == null) {
        e = "Địa chỉ không được để trống!";
    } else {
        //var mailformat = /^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)/;
        //if (un.value.match(mailformat)) {

        //} else {
        //    e = "Địa chỉ không hợp lệ!";
        }
    }
    document.getElementById("errorDiaChi").innerHTML = e;
}

function kiemTraSDT() {
    const un = document.getElementById("sdt");
    var e = ".";
    if (un.value === "" || un.value == null) {
        e = "Số điện thoại không được để trống!";
    } else {
        //var mailformat = /^\d{10}$/;
        //if (un.value.match(mailformat)) {

        //} else {
        //    e = "Số điện thoại không hợp lệ!";
        }
    }
    document.getElementById("errorSDT").innerHTML = e;
}

function kiemTraEmail() {
    const un = document.getElementById("email");
    var e = ".";
    if (un.value === "" || un.value == null) {
        e = "Email không được để trống!";
    } else {
        //var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        //if (un.value.match(mailformat)) {

        //} else {
        //    e = "Email không hợp lệ!";
        }
    }

    document.getElementById("errorEmail").innerHTML = e;
}