
var date_to_str = gantt.date.date_to_str(gantt.config.task_date);
var today = new Date();

gantt.config.row_height = 36;
gantt.config.min_column_width = 90;
gantt.config.duration_unit = "minute";
gantt.config.scales = [
    { unit: "month", step: 1, format: "%F, %Y" },
    {
        unit: "week", step: 1, format: function (date) {
            return "Semana #" + gantt.date.getWeek(date);
        }
    },
    {
        unit: "day", step: 1, format: "%D", css: function (date) {
            if (!gantt.isWorkTime({ date: date, unit: "day" })) {
                return "weekend"
            }
        }
    }
];

gantt.config.duration_step = 60;
gantt.addMarker({
    start_date: today,
    css: "today",
    text: today.toLocaleString(),
    title: today.toLocaleString()
});

gantt.templates.tooltip_text = function (start, end, task) {
    return "<b>Código: </b>" + task.text +
        "<br/><b>Descripción: </b>" + task.descripcion +
        "<br/><b>Especialidad: </b>" + task.especialidad +
        "<br/><b>Operarios: </b>" + task.operarios +
        "<br/><b>Fecha Inicio: </b>" + start.toLocaleString() +
        "<br><b>Duración: </b>" + task.duration + " h" +
        "<br/><b>Fecha Fin: </b>" + end.toLocaleString();
};

gantt.attachEvent("onTaskDblClick", function (id, e) {
    var task = gantt.getTask(id)
    var msj = "Código: " + task.text +
        "\r\nDescripción: " + task.descripcion +
        "\r\nEspecialidad: " + task.especialidad +
        "\r\nOperarios: " + task.operarios +
        "\r\nFecha Inicio: " + task.start_date.toLocaleString() +
        "\r\nDuración: " + task.duration + " h" +
        "\r\nFecha Fin: " + task.end_date.toLocaleString();
    alert(msj);
});

gantt.templates.task_class = function (start, end, task) {
    var css = "";
    if (task.tipo === "P")
        css = "tarPro";
    if (task.tipo === "L")
        css = "tarLib";
    if (task.tipo === "I")
        css = "tarInc"
    if (task.tipo === "T")
        css = "tarFin";
    return css;
};


gantt.templates.leftside_text = function (start, end, task) {
    var texto = "";
    if (task.tipo === "P" || task.tipo === "T")
        texto = "<b>" + task.start_date.toLocaleTimeString('en-GB').substring(0, 5) + "</b>";
    else
        texto = "<div class='textoTarIzq'><b>" + task.start_date.toLocaleTimeString('en-GB').substring(0, 5) + "</b></div>";
    return texto;
};

gantt.templates.grid_header_class = function (columnName, column) {
    if (columnName == 'desc' || columnName == 'text' || columnName == 'inc' || columnName == 'sep')
        return "updColorH";
};

gantt.templates.grid_row_class = function (start, end, task) {
    if (task.text.substring(0, 3) === "GYM")
        return "updColor";
    return "updColorE"
};

gantt.templates.scale_row_class = function (scale) {
    return "updColorC";
}

gantt.config.drag_resize = false;
gantt.config.drag_progress = false;
gantt.config.drag_move = false;

gantt.templates.rightside_text = function (start, end, task) {
    var texto = "";
    if (task.tipo === "P" || task.tipo === "T")
        texto = "<b>" + task.end_date.toLocaleTimeString('en-GB').substring(0, 5) + "</b>";
    else
        texto = "<div class='textoTarDer'><b>" + task.end_date.toLocaleTimeString('en-GB').substring(0, 5) + "</b></div>";
    return texto;
};


gantt.config.grid_width = 540 + 38;

var options = {
    year: "2-digit",
    month: "2-digit",
    day: "2-digit",
    hour: "numeric",
    minute: "2-digit"
};

gantt.config.columns = [

    //{
    //   //name: "text", label: textFilter, width: 105, resize: true, tree: true, template: function (i) {
    //   //name: "text", label: "<input filterPT type='text' oninput=filtroPT(this.value) style='width:98px; margin-top: 5px; height: 23px;' />", width: 105, resize: true, tree: true, template: function (i) {
    //   name: "text", label: "PT", width: 105, resize: true, tree: true, template: myFunc 
    //       if (i.tipo === "L" || i.tipo === "I")
    //           return "";
    //       return i.text;
    //   }
    {
        name: "text", label: "P.T", tree: true, width: 90, resize: true, template: function (i) {
            if (i.tipo === "L" || i.tipo === "I")
                return "";
            if (i.text.substring(0, 3) === "GYM")
                return "<div class='important'>" + i.text + " </div>";
            return "<div class='importantM'>" + i.text + " </div>";
        }
    },

    {
        name: "inc", label: "", width: 23, template: function (i) {
            if (i.inconveniente)
                return '<div><img src="../img/alertqt2.png" style="width:17px;height:15px;"></div>';
            if (i.urgencia)
                return '<div><img src="../img/urg_qt_small.png" style="width:17px;height:15px;"></div>';
                //return '<div class="inc-indicator"></div>';
            
            return '<div></div>';
        }
    },
    {
        //name: "desc", label: "Descripcion", width: '*', resize: true, template: function (i) {
        //name: "desc", label: "<div>Descripción : <input filterDes type='text' oninput=filtroDes(this.value) style='width:220px; margin-top: 5px; height: 23px;' /></div>", width: 300, resize: true, template: function (i) {
        name: "desc", label: "DESCRIPCIÓN", width: 250, resize: true, template: function (i) {
            if ((i.tipo === "P" || i.tipo === "T") && i.text.substring(0, 3) === "GYM")
                return "<div class='desPT important'>" + i.actividad + "<br>" + i.start_date.toLocaleString('mn-MN', options) + " - " + i.end_date.toLocaleString('mn-MN', options) + "</div>";
            if ((i.tipo === "P" || i.tipo === "T") && i.text.substring(0, 3) != "GYM")
                return "<div class='desPT importantC'>" + i.actividad + "<br>" + i.start_date.toLocaleString('mn-MN', options) + " - " + i.end_date.toLocaleString('mn-MN', options) + "</div>";
            if ((i.tipo === "L" || i.tipo === "I") && i.text.substring(0, 3) === "GYM")
                if (i.tipo === "L")
                    return "<div class='desLI important'>" + i.start_date.toLocaleString('mn-MN', options) + " - " + i.end_date.toLocaleString('mn-MN', options) + "</div>";
                else
                    return "<div class='desLI important'>" + i.start_date.toLocaleString('mn-MN', options) + " - " + i.end_date.toLocaleString('mn-MN', options) + " TH : " + i.totalHoras +"</div>";
            if ((i.tipo === "L" || i.tipo === "I") && i.text.substring(0, 3) != "GYM")
                if (i.tipo === "L")
                    return "<div class='desLI importantC'>" + i.start_date.toLocaleString('mn-MN', options) + " - " + i.end_date.toLocaleTimeString('mn-MN', options) + "</div>";
                else
                    return "<div class='desLI importantC'>" + i.start_date.toLocaleString('mn-MN', options) + " - " + i.end_date.toLocaleTimeString('mn-MN', options) + " TH : " + i.totalHoras +"</div>";


        }
    },
    {
        name: "sep", label: "", width: 5, template: function (i) {
            return '<div></div>';
        }
    },
];

var NCampo = "";
var filterValue = "";
var delay;
var normalizedText;


//function filtroPT(value) {
//    NCampo = "T";
//    filterValue = value;
//    clearTimeout(delay);
//    delay = setTimeout(function () {
//        gantt.render();
//        gantt.$root.querySelector("[filterPT]").focus();

//    }, 200)
//}

//function filtroDes(value) {
//    NCampo = "D"
//    filterValue = value;
//    clearTimeout(delay);
//    delay = setTimeout(function () {
//        gantt.render();
//        gantt.$root.querySelector("[filterDes]").focus();

//    }, 200)
//}

//gantt.attachEvent("onBeforeTaskDisplay", function (id, task) {
//    if (!filterValue) return true;
//    if (NCampo == "D")
//        normalizedText = task.descripcion.toLowerCase() + task.area.toLowerCase();
//    else
//        normalizedText = task.text.toLowerCase();

//    var normalizedValue = filterValue.toLowerCase();
//    return normalizedText.indexOf(normalizedValue) > -1;
//});

//gantt.attachEvent("onGanttRender", function () {
//    if (NCampo == "D")
//        gantt.$root.querySelector("[filterDes]").value = filterValue;
//    else
//        gantt.$root.querySelector("[filterPT]").value = filterValue;
//});


var zoomConfig = {
    levels: [
        {
            name: "hour",
            scale_height: 35,
            min_column_width: 65,
            scales: [
                { unit: "day", format: "%D,%d %F" },
                { unit: "hour", step: 1, format: "%H:%i" }
            ]
        },
        {
            name: "day",
            scale_height: 27,
            min_column_width: 20,
            scales: [
                { unit: "day", step: 1, format: "%D,%d %F" }
            ]
        },
        {
            name: "week",
            scale_height: 50,
            min_column_width: 40,
            scales: [
                {
                    unit: "week", step: 1, format: function (date) {
                        var dateToStr = gantt.date.date_to_str("%d %M");
                        var endDate = gantt.date.add(date, +6, "day");
                        var weekNum = gantt.date.date_to_str("%W")(date);
                        return "Semana " + weekNum + ", " + dateToStr(date) + " - " + dateToStr(endDate);
                    }
                },
                { unit: "day", step: 1, format: "%j %D" }
            ]
        },
        {
            name: "month",
            scale_height: 50,
            min_column_width: 50,
            scales: [
                { unit: "month", format: "%F, %Y" },
                { unit: "week", format: "Semana %W" }
            ]
        }
    ],
    useKey: "ctrlKey",
    widthStep: 30,
    minColumnWidth: 40,
    maxColumnWidth: 60,
    trigger: "wheel",
    element: function () {
        return gantt.$root.querySelector(".gantt_task");
    }
};
gantt.ext.zoom.init(zoomConfig);
gantt.ext.zoom.setLevel("hour");
gantt.init("ganttPT");
gantt.load("ListadoPedidoTrabajoGantt");

//GANTT DE RECURSOS
ganttRec = Gantt.getGanttInstance();
ganttRec.config.row_height = 30;
ganttRec.config.min_column_width = 20;
ganttRec.config.duration_unit = "minute";
ganttRec.config.scales = [
    { unit: "month", step: 1, format: "%F, %Y" },
    {
        unit: "week", step: 1, format: function (date) {
            return "Semana #" + ganttRec.date.getWeek(date);
        }
    },
    {
        unit: "day", step: 1, format: "%D", css: function (date) {
            if (!ganttRec.isWorkTime({ date: date, unit: "day" })) {
                return "weekend"
            }
        }
    }
];


ganttRec.config.duration_step = 60;
ganttRec.addMarker({
    start_date: today,
    css: "today",
    text: "Ahora",
    title: "Ahora: " + date_to_str(today)
});


ganttRec.templates.task_class = function (start, end, task) {
    var css = "";
    if (task.tipo === "P")
        css = "tarPro";
    if (task.tipo === "L")
        css = "tarLibRes";
    if (task.tipo === "T")
        css = "tarFin";
    return css;
}

ganttRec.templates.grid_header_class = function (columnName, column) {
    if (columnName == 'text' || columnName == 'inc' || columnName == 'urg')
        return "updColorH";
};

ganttRec.templates.grid_row_class = function (start, end, task) {
    if (task.text.substring(0, 3) === "GYM")
        return "updColor";
    return "updColorE"
};

ganttRec.templates.scale_row_class = function (scale) {
    return "updColorC";
}

ganttRec.templates.task_text = function (start, end, task) {
    if (task.supervisor || task.tecnico || (task.tipo === "L"))
        return "";
    return task.text;
};

ganttRec.templates.tooltip_text = function (start, end, task) {
    return "<b>Fecha Inicio: </b>" + start.toLocaleString() +
        "<br/><b>Fecha Fin: </b>" + end.toLocaleString() +
        "<br><b>Duración: </b>" + task.duration + " h";
};

ganttRec.attachEvent("onTaskDblClick", function (id, e) {
    var task = ganttRec.getTask(id)
    var msj = "Fecha Inicio: " + task.start_date.toLocaleString() +
        "\r\nDuración: " + task.duration + " h" +
        "\r\nFecha Fin: " + task.end_date.toLocaleString();
    alert(msj);
});

ganttRec.config.grid_width = 540 + 38;

ganttRec.config.drag_resize = false;
ganttRec.config.drag_progress = false;
ganttRec.config.drag_move = false;


ganttRec.config.columns = [
    {
        name: "inc", label: "", width: 21, template: function (i) {
            if (i.inconveniente && (i.tipo == "P" || i.tipo == "T"))
                return '<div><img src="../img/alertqt2.png" style="width:17px;height:15px;"></div>';              
            return '<div></div>';
        }
    },
    {
        name: "urg", label: "", width: 21, template: function (i) {
            if (i.urgencia && (i.tipo == "P" || i.tipo == "T"))             
                return '<div class="inc-indicator"></div>';
            return '<div></div>';
        }
    },
    {

        name: "text", label: "TÉCNICOS", width: '345', resize: true, tree: true, template: function (i) {
            if (i.tipo === "L")
                return "";
            if (i.text.substring(0, 3) === "GYM")
                return "<div class='important'>" + i.text + " </div>";
            return "<div class='importantM'>" + i.text + " </div>";

        }
    },
];
ganttRec.ext.zoom.init(zoomConfig);
ganttRec.ext.zoom.setLevel("hour");
ganttRec.init("ganttRC");
ganttRec.load("ListadoRecursoGantt");