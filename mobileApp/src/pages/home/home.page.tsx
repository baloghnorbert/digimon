import './home.style.css';

import React, { useState, useEffect } from "react";
import {
    IonPage,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonList,
    IonListHeader,
    IonInfiniteScroll,
    IonInfiniteScrollContent
} from "@ionic/react";
import { Digimon } from '../../client/client';
import { WebAPI } from '../../services/webAPI';
import DigimonComponent from './digimon.compoent';

const HomePage: React.FC = (): JSX.Element => {

    const [digimons, setDigimons] = useState<Digimon[]>([]);
    const [page, setPage] = useState<number>(0);
    const [disableInfinitScroll, setDisableInfinitScroll] = useState<boolean>(false);
    const [isLoading, setIsLoading] = useState<boolean>(true);

    useEffect(() =>{
        fetchData();
        setIsLoading(false);

    },[]);

    const searchNext = async (e: CustomEvent<void>): Promise<void> => {
        await fetchData();
        (e.target as HTMLIonInfiniteScrollElement).complete();
    }

    const fetchData = async (): Promise<void> => {
        const data: Digimon[] = await WebAPI.Digimons.page(page);
        if (data && data.length > 0) {
            setDigimons([...digimons, ...data]);

            if (data.length < 10) //kiolvastuk az összeset, már nincs több benne
            {
                setDisableInfinitScroll(true);
            } else {
                setPage(page + 1);
            }
        } else {
            setDisableInfinitScroll(true);
        }
    }

    return (
        <IonPage>
            <IonHeader>
                <IonToolbar>
                    <IonTitle>STAR WARS CHARACHTERS</IonTitle>
                </IonToolbar>
            </IonHeader>
            <IonContent fullscreen>
                <IonList>
                    <IonListHeader></IonListHeader>
                    {
                        digimons.map((data,index) => <DigimonComponent digimon={data} key={index} />)
                    }
                </IonList>
                <IonInfiniteScroll
                    onIonInfinite={e => searchNext(e)}
                    threshold="50px"
                    disabled={disableInfinitScroll}
                >
                    <IonInfiniteScrollContent
                        loadingSpinner="bubbles"
                        loadingText="Loading more data..."
                    >

                    </IonInfiniteScrollContent>
                </IonInfiniteScroll>
            </IonContent>
        </IonPage>
    );
}
export default HomePage;