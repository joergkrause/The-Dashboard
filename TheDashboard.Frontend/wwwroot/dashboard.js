window.blazorHelpers = {
  lastElementId: '',
  scrollToFragment: (elementId) => {
    if (this.lastElementId !== elementId) {
      this.lastElementId = elementId;
      var element = document.querySelector(`[name="${elementId}"]`);
      if (element) {
        var rect = element.getBoundingClientRect();
        window.scrollTo({ top: parseInt(rect.y.toString()) + window.scrollY - 90, left: 0, behavior: "smooth" });
      }
    }
  },
  width: 320,
  height: 0,
  streaming: false,
  video: null,
  canvas: null,
  photo: null,
  cameraOptions: null,
  snapbbtn: null,
  isInitalized: false,
  initMedia: async () => {
    if (blazorHelpers.isInitalized) return;
    blazorHelpers.isInitalized = true;
    await this.blazorHelpers.enumerateDevices();
    console.log("Init Media");
    this.blazorHelpers.video = document.getElementById('cam');
    this.blazorHelpers.video.addEventListener(
      "canplay",
      (ev) => {
        if (!this.blazorHelpers.streaming) {
          this.blazorHelpers.height = (this.blazorHelpers.video.videoHeight / this.blazorHelpers.video.videoWidth) * this.blazorHelpers.width;
          this.blazorHelpers.video.setAttribute("width", this.blazorHelpers.width);
          this.blazorHelpers.video.setAttribute("height", this.blazorHelpers.height);
          this.blazorHelpers.canvas.setAttribute("width", this.blazorHelpers.width);
          this.blazorHelpers.canvas.setAttribute("height", this.blazorHelpers.height);
          this.blazorHelpers.streaming = true;
        }
      },
      false
    );
    this.blazorHelpers.canvas = document.getElementById('canvas');
    this.blazorHelpers.photo = document.getElementById('photo');
    this.blazorHelpers.snapbtn = document.getElementById('snapbtn');
    this.blazorHelpers.snapbtn.addEventListener(
      "click",
      (ev) => {
        this.blazorHelpers.snapMedia();
        ev.preventDefault();
      },
      false
    );
    await this.blazorHelpers.startMedia();
  },
  startMedia: async () => {
    try {
      if ('mediaDevices' in navigator && 'getUserMedia' in navigator.mediaDevices) {
        var stream = await navigator.mediaDevices.getUserMedia({
          video: {
            width: {
              min: 720,
              ideal: 720,
              max: 1440
            },
            height: {
              min: 960,
              ideal: 960,
              max: 1280,
            },
            deviceId: {
              exact: this.blazorHelpers.cameraOptions.value
            }
          }
        });
        this.blazorHelpers.video.srcObject = stream;
        this.blazorHelpers.video.play();
      }
    }
    catch (e) {
      console.log("Exception occurred", e);
    }
  },
  stopMedia: () => {
    const stream = this.blazorHelpers.video.srcObject;
    if (stream) {
      stream.getTracks().forEach((track) => {
        track.stop();
      });
    }
  },
  getMedia: async (dotnetHelper) => {
    let dataUrl = this.blazorHelpers.canvas.toDataURL("image/jpeg");
    await dotnetHelper.invokeMethodAsync('SaveImage', dataUrl);
  },
  snapMedia: async () => {
    console.log("Snap Media");
    try {
      const context = this.blazorHelpers.canvas.getContext("2d");
      if (this.blazorHelpers.width && this.blazorHelpers.height) {
        this.blazorHelpers.canvas.width = this.blazorHelpers.width;
        this.blazorHelpers.canvas.height = this.blazorHelpers.height;
        context.drawImage(this.blazorHelpers.video, 0, 0, this.blazorHelpers.width, this.blazorHelpers.height);

        const data = this.blazorHelpers.canvas.toDataURL("image/png");
        this.blazorHelpers.photo.setAttribute("src", data);
      } else {
        this.blazorHelpers.clearMedia();
      }
    }
    catch (e) {
      console.log("Exception occurred", e);
    }
  },
  clearMedia: () => {
    const context = this.blazorHelpers.canvas.getContext("2d");
    context.fillStyle = "#AAA";
    context.fillRect(0, 0, this.blazorHelpers.canvas.width, this.blazorHelpers.canvas.height);
    const data = this.blazorHelpers.canvas.toDataURL("image/png");
    this.blazorHelpers.photo.setAttribute("src", data);
  },
  enumerateDevices: async () => {
    if (!this.blazorHelpers.cameraOptions) {
      // enforce calling for cam so deviceId get filled
      await navigator.mediaDevices.getUserMedia({
        audio: false,
        video: {
          width: {
            min: 720,
            ideal: 720,
            max: 1440
          }
        }
      });
      const devices = await navigator.mediaDevices.enumerateDevices();
      const videoDevices = devices.filter(device => device.kind === 'videoinput');
      let first = true;
      const options = videoDevices.map(videoDevice => {
        var label = videoDevice.label ? videoDevice.label : 'Kamera';
        if (label.indexOf('back') > -1) {
          label = 'Rückkamera';
        }
        if (label.indexOf('front') > -1) {
          label = 'Frontkamera (Selfie)';
        }
        var option = `<option ${first ? 'selected' : ''} value="${videoDevice.deviceId}">${label}</option>`;
        first = false;
        return option;
      });
      this.blazorHelpers.cameraOptions = document.getElementById('camselect');
      this.blazorHelpers.cameraOptions.innerHTML = options.join('');
      this.blazorHelpers.cameraOptions.addEventListener('change', async () => {
        this.blazorHelpers.stopMedia();
        this.blazorHelpers.startMedia();
      });
    }
  }
};
window.downloadFileFromStream = async (fileName, contentStreamReference) => {
  const arrayBuffer = await contentStreamReference.arrayBuffer();
  const blob = new Blob([arrayBuffer]);
  const url = URL.createObjectURL(blob);
  const anchorElement = document.createElement('a');
  anchorElement.href = url;
  anchorElement.download = fileName ?? '';
  anchorElement.click();
  anchorElement.remove();
  URL.revokeObjectURL(url);
};
window.toggleEditMode = (element, state) => {
  element.setAttribute('contenteditable', state);
};
window.getContent = (element) => {
  return element.innerHTML;
};
window.consent = {
  acceptMessage: (cookieString) => {
    document.cookie = cookieString;
  }
};
window.removeHelpers = () => {
  document.querySelectorAll('.popover').forEach((e, idx) => {
    e.classList.remove('show');
  })
}
window.removeHelpersDelayed = () => {
  setTimeout(() => {
    window.removeHelpers();
  }, 7000);
}
window.document.addEventListener('click', () => {
  window.removeHelpers();
});
window.showHelper = (elementRef) => {
  elementRef.classList.add("show");
};
window.mask = (id, mask, isRegEx, returnRawValue, dotnetHelper) => {
  var pattern;
  if (isRegEx)
    pattern = new RegExp(mask);
  else
    pattern = mask;
  var customMask = IMask(
    document.getElementById(id), {
    mask: pattern,
    overwrite: true,
    commit: function (value, masked) {
      if (returnRawValue === true)
        dotnetHelper.invokeMethodAsync('returnCurrentValue', this.unmaskedValue);
      else
        dotnetHelper.invokeMethodAsync('returnCurrentValue', value);

    }
  });
  // for tablet view
  window.openfullScreen = (URL) => {
    window.open(URL, '', 'fullscreen=yes, scrollbars=auto');
  };
};