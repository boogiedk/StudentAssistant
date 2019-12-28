import axios from 'axios';

const backendUrl = 'http://localhost:18935';

export default class RestService {

    get(path, params) {
       return axios.get(this.url(backendUrl, path), {
            headers: this.headers(),
            params: params
        })
            .then(response => {
                return response;
            }) 
            .catch((error) => {
                console.log(error);
            });
    }

    post(path, body) {
        return axios.post(this.url(backendUrl, path), body, {
            headers: this.headers()
        })
            .then(function (response) {
                return response;
            })
            .catch(function (error) {
                console.log(error);
            });
    }

    url(url, path) {
        return url + path;
    }

    headers() {
        return {
            'Accept': ' application/json',
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': '*',
        };
    }
}

