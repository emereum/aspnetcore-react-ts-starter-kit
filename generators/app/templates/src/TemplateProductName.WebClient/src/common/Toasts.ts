import { toast } from "react-toastify";
import { style } from "react-toastify";

style({
  width: "600px"
});



let networkErrorToastId: number | undefined;
export const networkError = () => {
  if(networkErrorToastId == null || !toast.isActive(networkErrorToastId)) {
    networkErrorToastId = toast.warn("A network error occurred. Please wait a few moments then try again.");
  }
};