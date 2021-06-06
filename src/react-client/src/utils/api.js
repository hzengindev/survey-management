import axios from "axios";
import { APIURL } from "./contants";

const api = axios.create();
api.interceptors.request.use(
  (config) => {
    config.baseURL = APIURL;

    if (config.url.indexOf("/image/") > -1) {
      config.responseType = "blob";
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => {
    return response;
  },
  function (error) {
    return Promise.reject(error);
  }
);

export default api;
