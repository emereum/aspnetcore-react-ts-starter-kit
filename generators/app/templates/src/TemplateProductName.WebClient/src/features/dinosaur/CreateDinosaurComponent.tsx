import * as React from "react";
import { observer } from "mobx-react";
import BindConfig from "../../common/BindConfig";
import CreateDinosaurStore from "./CreateDinosaurStore";
import { Button } from "semantic-ui-react";
import GeneralErrors from "../../common/GeneralErrors";
import PropertyErrors from "../../common/PropertyErrors";

@observer
export default class CreateDinosaurComponent extends React.Component<{}, {}> {
    render() {
        const { dinosaur, createDinosaur, createDinosaurResult } = CreateDinosaurStore;

        var bind = BindConfig({
            context: dinosaur
        });

        return (
            <div>
                <h1>Create Dinosaur</h1>
                <GeneralErrors errors={createDinosaurResult.data} guid={dinosaur.guid} />

                <label>Name:</label>
                <PropertyErrors errors={createDinosaurResult.data} guid={dinosaur.guid} errorProperty="name">
                    <input type="text" {...bind("name")} />
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