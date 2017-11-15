import * as React from "react";
import { observer } from "mobx-react";
import CreateDinosaurStore from "./CreateDinosaurStore";

@observer
export default class CreateDinosaurComponent extends React.Component<{}, {}> {
    handleChangeDinosaurName = (e: React.FormEvent<HTMLInputElement>) => {
        CreateDinosaurStore.dinosaur.name = e.currentTarget.value;
    }

    render() {
        const { dinosaur } = CreateDinosaurStore;

        return (
            <div>
                <h1>Create Dinosaur</h1>
                <label>Name:</label>
                <input type="text" value={dinosaur.name} onChange={this.handleChangeDinosaurName} />
            </div>
        );
    }
}