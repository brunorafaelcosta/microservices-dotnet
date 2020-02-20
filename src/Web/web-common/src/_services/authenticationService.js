import { BehaviorSubject } from 'rxjs';
import axios from '../utils/axios';

// import { handleResponse } from '../utils/handleResponse';

const currentUserSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('currentUser')));

const authenticationService = {
    getAuthenticatedUser,
    logout
};

function getAuthenticatedUser() {
    return new Promise(function (resolve, reject) {
        axios
            .get('/api/getauthenticateduser')
            // .then(handleResponse)
            .then(user => {
                localStorage.setItem('currentUser', JSON.stringify(user.data))
                setTimeout(() => resolve(user), 1000)
            })
            .catch(function (error) {
                reject(error)
            })
    })
}

function logout() {
    return new Promise(function (resolve, reject) {
        localStorage.removeItem('currentUser')
        resolve(true)
    })
}

export default authenticationService;