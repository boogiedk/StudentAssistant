import RestService from "./RestService";

const restService = new RestService();

export default class CourseScheduleService {

    get(selectedDatetime, groupName) {
        let path = '/api/v1/schedule/selected';

        let requestModel = {
            dateTimeRequest: selectedDatetime,
            groupName: groupName
        };

       return restService.post(path, requestModel);
    }
    
    update() {
        let path = '/api/v1/schedule/download';

        restService.get(path)
            .then(json => console.log(json));
    }
}

