import { observable } from "mobx";
import { toast } from "react-toastify";
import Dinosaur from "../../model/Dinosaur";
import Api from "../../services/Api";
import ValidationErrors from "../../common/ValidationErrors";
import Loadable from "../../services/Loadable";

class CreateDinosaurStore {
    @observable dinosaur: Dinosaur = new Dinosaur();
    @observable createDinosaurResult = new Loadable<ValidationErrors>();

    createDinosaur = () => {
        Api
            .post("/dinosaurs", this.dinosaur, this.createDinosaurResult)
            .then(response => {
                if(response.ok) {
                    toast("Congratulations, We did the thing!");
                }
            });
    }
}

export default new CreateDinosaurStore();