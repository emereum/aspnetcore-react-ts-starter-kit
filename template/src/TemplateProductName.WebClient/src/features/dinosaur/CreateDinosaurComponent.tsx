import { observer } from "mobx-react";
import * as React from "react";
import { Button } from "semantic-ui-react";
import BindConfig from "../../common/BindConfig";
import GeneralErrors from "../../common/GeneralErrors";
import PropertyErrors from "../../common/PropertyErrors";
import CreateDinosaurStore from "./CreateDinosaurStore";

@observer
export default class CreateDinosaurComponent extends React.Component<{}, {}> {
    render() {
        const { dinosaur, createDinosaur, createDinosaurResult } = CreateDinosaurStore;

        const bind = BindConfig({
            context: dinosaur
        });

        return (
            <div>
                <h1>Create Dinosaur</h1>
                <GeneralErrors errors={createDinosaurResult.data} id={dinosaur.id} />

                <label>Name:</label>
                <PropertyErrors errors={createDinosaurResult.data} id={dinosaur.id} errorProperty="name">
                    <input type="text" {...bind("name")} disabled={createDinosaurResult.isLoading} />
                </PropertyErrors>
                <Button
                    type="submit"
                    disabled={createDinosaurResult.isLoading}
                    loading={createDinosaurResult.isLoading}
                    content="Create"
                    onClick={createDinosaur}
                />
            </div>
        );
    }
}