import * as React from "react";
import { toast } from "react-toastify";

interface IErrorBoundaryProps {
    children: JSX.Element;
}

interface IErrorBoundaryState {
    somethingWentWrong: boolean;
}

/**
 * An ErrorBoundary component which either renders its children or renders an
 * error toast message if any of the children throw an exception upon rendering.
 * See: https://reactjs.org/blog/2017/07/26/error-handling-in-react-16.html
 */
export default class ErrorBoundaryComponent extends React.Component<IErrorBoundaryProps, IErrorBoundaryState> {
    constructor(props: IErrorBoundaryProps) {
        super(props);
        this.state = { somethingWentWrong: false };
    }

    componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
        this.setState({ somethingWentWrong: true });
        toast("Something went wrong! Please refresh the page and try again.", {
            autoClose: false,
            type:"error",
            closeButton: false
        });
    }

    render() {
        const { somethingWentWrong } = this.state;
        const { children } = this.props;

        return somethingWentWrong
            ? <div />
            : children;
    }
}