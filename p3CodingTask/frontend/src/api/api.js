﻿import axios from "axios";
const api = axios.create({
  baseURL: "http://localhost:5000/api/fileshare",
});
api.defaults.headers.post["Content-Type"] = "application/json";
api.defaults.headers.put["Content-Type"] = "application/json";
api.defaults.headers.get["Content-Type"] = "application/json";
export default {
  async getFolderContents(url) {
    const response = await api.get(`/folder?url=${url}`);
    return response.data;
  },
  async createFolder(path) {
    const response = await api.post(`/new-folder`, path);
    return response.data;
  },
  async uploadFiles(files, folderPath) {
    const formData = new FormData();
    formData.append("folderPath", folderPath);

    for (let i = 0; i < files.length; i++) {
      formData.append("files", files[i]);
    }
    const response = await api.post(`/files`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return response.statusText;
  },
  async deleteResource(path) {
    const response = await api.delete(`/delete`, path);
    return response.data;
  },
  async searchTopN(query) {
    const response = await api.get(`/search`, query);
    return response.data;
  },
  async shareResource(resourcePath) {
    const response = await api.get(`/share`, resourcePath);
    return response.data;
  },
};
