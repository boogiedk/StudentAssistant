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

        restService.get(path)
            .then(json => console.log(json));
    }

    validate(controlWeekModel) {
        if ((typeof controlWeekModel === "undefined") ||
            (controlWeekModel === null))
            return false;

        return true;
    }
    
}

