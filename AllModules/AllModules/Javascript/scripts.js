function ShowHide(id) {
    var el = document.getElementById('AdvSearch');
    var expcol = document.getElementById('imgA');
    if (el.style.display == 'none') {
        el.style.display = 'block';
        /*expcol.src = "Images/imgUblue.png" */
        expcol.src = "../../Images/imgUblue.PNG"
    }
    else {
        el.style.display = 'none';
        /* expcol.src = "Images/imgDblue.png" */
        expcol.src = "../../Images/imgDblue.PNG"
    }
}

function DoSmth() {
    return Page_ClientValidate('ALL');
}

function parseDate(str) {
    var m = str.match(/^(\d{1,2})-(\d{1,2})-(\d{4})$/);
    return (m) ? new Date(m[3], m[2] - 1, m[1]) : null;
}
function ValidateDateOf(obj) {
    DayObj = new Date();
    var txtdate = obj.value;
    var arr = txtdate.split("/");
    var leapday = arr[0];
    var leapmonth = arr[1];
    var leapyear = arr[2];

    var CurDate = DayObj.getDate();
    var CurYear = DayObj.getFullYear();
    var CurMonth = DayObj.getMonth();

    if (leapday == '') {
        alert('day Field is required');
        return false;
    }
    if (leapmonth == '') {
        alert('month Field is required');
        return false;
    }
    if (leapyear == '') {
        alert('year Field is required');
        return false;
    }
    if (parseInt(leapday) > 29) {
        alert('invalid date');
        return false;
    }
    if (!isleap(leapyear)) {
        if (parseInt(leapday) > 28) {
            alert('invalid date');
            return false;
        }
        return true;
    }
    return true;
}
function isleap(leapyear) {
    var yr = leapyear;
    if ((parseInt(yr) % 4) == 0) {
        if (parseInt(yr) % 100 == 0) {
            if (parseInt(yr) % 400 != 0) {
                return false;
            }
            if (parseInt(yr) % 400 == 0) {
                return true;
            }
        }
        if (parseInt(yr) % 100 != 0) {
            return true;
        }
    }
    if ((parseInt(yr) % 4) != 0) {
        return false;
    }
}