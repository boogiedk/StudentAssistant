import RestService from "./RestService";
import ToastNotificationService from "./ToastNotificationService";
import {history} from "./../helpers/history"

const restService = new RestService();
const toastNotificationService = new ToastNotificationService();

export default class AccountService {
    login(login, password) {

        let requestModel = {
            login: login,
            password: password
        };

        if (!this.validateRequest(requestModel)) {
            toastNotificationService.notifyError("Некорректные данные.");
            return {
                success: false
            };
        }

        let path = '/api/v1/account/login';

        return restService.post(path, requestModel)
            .then(response => {
                if (this.validateResponse(response)) {
                    return {
                        success: true
                    };
                } else {
                    toastNotificationService.notify(response.status, "Unauthorized.");
                    return {
                        success: false
                    };
                }
            });
    }

    register(registerUser) {
        let path = '/api/v1/account/register';

        if (!this.validateRequest(registerUser)) {
            toastNotificationService.notifyError("Некорректные данные.");
            return {
                success: false
            };
        }

        return restService.post(path, registerUser)
            .then(response => {
                if (this.validateResponse(response)) {
                    
                    return {
                        success: true
                    };
                } else {
                    toastNotificationService.notifyErrorList(response.status, response.data);
                    return {
                        success: false
                    };
                }
            });
    }

    logout() {
        history.push('/login');
        let path = '/api/v1/account/logout';
        return restService.get(path).then(response => {
            if (this.validateResponse(response)) {
                return {
                    success: true
                };
            } else {
                return {
                    success: false
                };
            }
        });
    }

    getProfile()
    {
        let path = '/api/v1/account/get';

        return restService.get(path)
            .then(response => {
                console.log(response);
                if (this.validateResponse(response)) {
                    return {
                        user:response.data,
                        success: true
                    };
                } else {
                    toastNotificationService.notify(response.status, "Unauthorized.");
                    return {
                        success: false
                    };
                }
            });
    }


    validateRequest(user) {
        if (user === null || typeof user === 'undefined') {
            return false;
        }

        if (user.login === null || typeof user.login === 'undefined' || user.login === '') {
            return false;
        }

        if (user.password === null || typeof user.password === 'undefined' || user.password === '') {
            return false;
        }

        return true;
    }


    isAuth() {
        let path = '/api/v1/account/isAuth';
        return restService.get(path).then(response => {
            if (this.validateResponse(response)) {
                return {
                    success: true
                };
            } else {
                return {
                    success: false
                };
            }
        });
    }

    validateResponse(response) {
        if (response === null || typeof response === 'undefined') {
            return false;
        }

        switch (response.status) {
            case 500:
            case 404:
            case 400:
            case 401:
                return false;

            case 200:
                return true;


            default:
                return false;
        }
    }
}


