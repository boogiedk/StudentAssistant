import RestService from "./RestService";

const restService = new RestService();

export default class ControlWeekService {

    get(groupName) {
        let path = '/api/v1/controlWeek/Get';

        let requestModel = {
            groupName: groupName
        };

        return restService.post(path, requestModel);
    }

    update() {
        let path = '/api/v1/controlWeek/download';

        return restService.get(path);
    }

    validate(controlWeekModel) {
        if ((typeof controlWeekModel === "undefined") ||
            (controlWeekModel === null))
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

