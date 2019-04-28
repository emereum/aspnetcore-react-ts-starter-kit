import { toast } from "react-toastify";

/**
 * Ensures that only one instance of a particular toast is visible at a time
 * even if it is called several times in a short period.
 */
const deduplicate = (makeToast: () => number) => {
  let toastId: number | undefined;
  return () => {
    if(toastId == null || !toast.isActive(toastId)) {
      toastId = makeToast();
    }
  };
};

export const networkError = deduplicate(() => toast.warn("A network error occurred. Please wait a few moments then try again."));