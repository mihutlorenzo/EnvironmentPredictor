package com.example.sensorsapplication;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.http.GET;
import retrofit2.http.Path;

public interface ApiInterface {
    @GET("/api/buildModel")
    Call<String> BuildModel();

    //i.e. http://localhost:56983/api/buildModel/deleteTrainingData
    @GET("/api/buildModel/deleteTrainingData")
    Call<String> DeleteTrainingData();

    @GET("/api/values")
    Call<String[]> GetValues();

    //i.e. http://localhost:56983/api/gatherData/startGatheringTrainingData
    @GET("/api/gatherData/startGatheringTrainingData")
    Call<String> StartGatherTrainingData();

    //i.e. http://localhost:56983/api/gatherData/writeEnvironmentState
    @GET("/api/gatherData/writeEnvironmentState/{state}")
    Call<String> SetUpEnvironmentState(@Path("state") String environmentState);

    //i.e. http://localhost:56983/api/gatherData/stopGatheringTrainingData
    @GET("/api/gatherData/stopGatheringTrainingData")
    Call<String> StopGatheringTrainingData();


    //i.e. http://localhost:56983/api/prediction/startPredictingOnTestData
    @GET("/api/prediction/startPredictingOnTestData")
    Call<String> StartPredictingOnTestData();

    //i.e. http://localhost:56983/api/prediction/getPredictedValue
    @GET("/api/prediction/getPredictedValue")
    Call<String> GetPredictedValue();

    //i.e. http://localhost:56983/api/prediction/stopPredictingOnTestData
    @GET("/api/prediction/stopPredictingOnTestData")
    Call<String> StopPredictingOnTestData();
}
