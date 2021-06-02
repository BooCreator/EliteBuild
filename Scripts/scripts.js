// показаль элемент по селектору
function Show(select) {
    let elem = $(select);
    $(elem).addClass("show");
}
// скрыть элемент по селектору
function Hide(select) {
    let elem = $(select);
    $(elem).removeClass("show");
}
// показать работы выбранной категории по селектору
function ShowCategory(select) {
    $(".category.show").removeClass("show");
    $(select).addClass("show");
}
// показать картинку во весь экран
function Lightbox(elem) {
    let src = $(elem).find("img").attr("src");
    let image = $("#lightbox_image").attr("src", src);
    $(".lightbox").addClass("show");
}
// вывести сообщениена экран
function msg(message) {
    let i_class = "alert alert-dismissible fade show ";
    var text = message;
    switch (message) {
        case 'ok':
            i_class += "alert-success";
            text = "Операция выполнена успешно!";
            break;
        case 'exist':
            i_class += "alert-danger";
            text = "Объект уже существует!";
            break;
        case 'empty':
            i_class += "alert-danger";
            text = "Объект не найден!";
            break;
        case 'not_access':
            i_class += "alert-danger";
            text = "Доступ запрещен!";
            break;
        case 'unknown_error':
            i_class += "alert-danger";
            text = "Произошла неизвестная ошибка!";
            break;
        default:
            i_class += "alert-warning"; break;
    }
    alert(text);
   
}
// добавить пустой блок на странице редактирования страниц
function add_edit() {
    $("#items").append(
        '<div class="edit">'+
            '<div class="type">' +
                $($(".type")[0]).html() +
            '</div>'+
            '<textarea maxlength=\"8000\"></textarea>'+
        '</div>'
    );
}
// добавить пустую комнату в калькуляторе
function add_room() {
    $(".check_room").append(
        '<div class= "room" >' +
        '<label class="form-label title">Помещение</label>' +
        '<div>' +
            '<label for="validationCustom01" class="form-label">Площадь(М²)</label>' +
            '<input type="text" class="form-control sqare" value="" onchange="SumAll()">' +
                '<div class="invalid-feedback"></div>' +
                '</div>' +
            '<div>' +
                '<label for="validationCustom01" class="form-label">Высота (М)</label>' +
                '<input type="text" class="form-control height" value="" onchange="SumAll()">' +
                    '<div class="invalid-feedback"></div>' +
                '</div>' +
            '</div>'
    );
}
// удалить блок на странице редактирования страниц
function remove_edit(elem) {
    if ($(".edit").length > 1)
        $(elem).closest(".edit").remove();
    else
        msg("На странице должен быть как минимум один блок!");
}
// отправить post запрос
function post(action, value) {
    $.post(action, { id: value }).done(
        function (data) {
            msg(data);
            location.reload();
        });
}
//отправить информацию страницы после редактирования/добавления
function send_page(action, item_id) {
    let value = $("#Title").val();
    let submenu = $("#submenu").val();
    let elems = $(".edit");
    let items = [];
    for (let i = 0; i < elems.length; i++) {
        let type = $(elems[i]).children(".type").children("select").val();
        let text = $(elems[i]).children("textarea").val();
        items[i] = {
            Type: type,
            Text: text.replaceAll("<", "&code_lt;").replaceAll(">", "&code_gt;")
        };
    }
    let item = {
        Id: item_id,
        Title: value,
        Submenu: submenu,
        Items: items
    };
    $.post(action, { Json: JSON.stringify(item) }).done(
        function (data) {
            msg(data);
            if(item_id == -1)
                location.href = "/Page/LastPage";
            else
                location.href = "/Page/Index/" + item_id;
        });
}
// рассчитать общую сумму работ на странице калькулятора
function SumAll() {
    let price = $('input[name="radio-work"]:checked').val();
    if (price === undefined) {
        price = 0;
    }
    let items = $(".room");
    let sum = 0;
    for (let i = 0; i < items.length; i++) {
        let input1 = $(items[i]).find(".sqare").val();
        let input2 = $(items[i]).find(".height").val();;
        if (input1 > 0 && input2 > 0) {
            sum += price * input1 + price * input2;
        }
    }
    $("#price").text(sum + " руб.");
}
// отправить заявку
function send_request(action) {
    let name = $("#fio_name").val();
    let phone = $("#phone").val();
    let calculalte = "";
    let category = $('input[name="radio-stacked"]:checked').attr("id");
    let work = $('input[name="radio-work"]:checked').attr("id");
    if (work !== undefined) {
        let price = $('#price').html();
        if (price === undefined) {
            price = 0;
        }
        price = price.replace(" руб.", "");
        calculalte += "[" + category.replace("category_", "") + "]";
        calculalte += "[" + work.replace("work_", "") + "]";
        calculalte += "[" + price + "]";
        let items = $(".room");
        for (let i = 0; i < items.length; i++) {
            let input1 = $(items[i]).find(".sqare").val();
            let input2 = $(items[i]).find(".height").val();;
            if (input1 > 0 && input2 > 0) {
                calculalte += "[" + input1 + "," + input2 + "]";
            }
        }
    }
    $.post(action, { Name: name, Phone: phone, Calculate: calculalte }).done(
        function (data) {
            msg(data);
            location.reload();
        });
}
// обновить состояние запроса
function update_ReqState(action, request, elem) {
    let state = $(elem).val();
    $.post(action, { Id: request, State: state }).done(function (data) { });
}
// обновить состояние запроса, с указанием конкретного значения
function update_ReqState_Value(action, request, state) {
    $.post(action, { Id: request, State: state }).done(function (data) { });
}

// обновить блок в подвале для страницы
function update_PageBlock(action, request, elem) {
    let block = $(elem).val();
    $.post(action, { Id: request, Block: block }).done(function (data) { });
}
// обновить блок в подвале для страницы, с указанием конкретного значения
function update_PageBlock_value(action, request, block) {
    $.post(action, { Id: request, Block: block }).done(function (data) { });
}

// удаление страницы по ID установленному в #page
function remove(action) {
    let value = $("#page").val();
    post(action, value);
}

// показаль элемент по селектору и установить значение
function Show_Value(select, value) {
    $("#page").val(value);
    let elem = $(select);
    $(elem).addClass("show");
}