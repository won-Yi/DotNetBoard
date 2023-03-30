class UploadAdapter {
    constructor(loader, url) {
        this.loader = loader;
        this.url = url;
    }

    upload() {
        return this.loader.file.then(file => new Promise((resolve, reject) => {
            let formData = new FormData();
            formData.append('file', file);

            fetch(this.url, {
                method: 'POST',
                body: formData,
            })
                .then(response => {
                    if (response.ok) {
                        response.json().then(data => {
                            resolve({ default: data.url });
                        });
                    } else {
                        reject(`서버에서 오류가 발생했습니다. (${response.status})`);
                    }
                })
                .catch(error => {
                    reject('파일 업로드 중 오류가 발생했습니다.');
                });
        }));
    }

    abort() {
        // 업로드가 취소되는 경우에 수행할 작업을 여기에 작성합니다.
    }
}