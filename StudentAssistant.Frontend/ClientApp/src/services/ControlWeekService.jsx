import RestService from "./RestService";
import ToastNotificationService from "./ToastNotificationService";

const restService = new RestService();
const toastNotificationService = new ToastNotificationService();

export default class ControlWeekService {

    get(groupName) {
        let path = '/api/v1/controlWeek/Get';

        let requestModel = {
            groupName: groupName
        };

        return restService.post(path, requestModel).then(response => {

            if (this.validateResponse(response)) {
                //если ок
                return {
                    controlWeekModel: response.data,
                    loading: false
                };

            } else {
                //если с ошибкой
                toastNotificationService.notify(500, "Произошла какая-то ошибка :(");
                return {
                    controlWeekModel: {},
                    loading: true
                };
            }
        });
    }

    update() {
        let path = '/api/v1/controlWeek/update';

        restService.get(path)
            .then(response => {

                if (this.validateResponse(response)) {
                    toastNotificationService.notify(response.status, response.data.message)
                } else {
                    toastNotificationService.notify(500, "Произошла какая-то ошибка :C")
                }
            });
    }

    validateControlWeekModel(controlWeekModel) {
        if ((typeof controlWeekModel === "undefined") ||
            (controlWeekModel === null))
            return false;

        return true;
    }

    validateResponse(response) {

        if ((typeof response === "undefined") ||
            (response === null))
            return false;

        return true;
    }
    

    prepareNameOfDayWeek(nameOfDayWeek) {
        if (nameOfDayWeek === null || typeof nameOfDayWeek === "undefined")
            return nameOfDayWeek;

        // размер минимального имени дня недели, 
        // который помещается в таблицу
        if (nameOfDayWeek.length > 7) {
            return nameOfDayWeek.substring(0, 5) + ".";
        } else {
            return nameOfDayWeek;
        }
    }

}

