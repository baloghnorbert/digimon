import React from "react";

import { IonItem, IonAvatar, IonLabel } from "@ionic/react";
import { Digimon } from "../../client/client";

interface IProps {
    digimon: Digimon;
}

const DigimonComponent: React.FC<IProps> = (props: IProps): JSX.Element => {

    return (
        <IonItem routerLink={`/detail/${props.digimon.id}`}>
            <IonAvatar slot="start">
                <img src={props.digimon.img} alt={props.digimon.name} />
            </IonAvatar>
            <IonLabel>
                <h2>{props.digimon.name}</h2>
                <h3>{props.digimon.level}</h3>
            </IonLabel>
        </IonItem>
    );
}

export default DigimonComponent;