package com.example.sensorsapplication;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {
    private Button gatherTrainingDataButton;
    private Button buildModelButton;
    private Button deleteTrainingDataButton;
    private Button startPredictionButton;
    private Button stopPredictionButton;
    private Button getPredictedValuesButton;

    ApiInterface apiService;
    Call<String> callBuildModel;
    Call<String> callDeleteTrainingData;
    Call<String> callStartPrediction;
    Call<String> callStopPrediction;
    Call<String> callGetPredictedValue;
    private static final String TAG = MainActivity.class.getSimpleName();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        apiService = ApiClient.getClient();


        gatherTrainingDataButton = (Button) findViewById(R.id.gatherTrainingData);
        gatherTrainingDataButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openCollectTrainingDataActivity();
            }
        });

        buildModelButton = (Button) findViewById(R.id.buildModel);
        buildModelButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                buildModel();
            }
        });

        deleteTrainingDataButton = (Button) findViewById(R.id.deleteTrainingData);
        deleteTrainingDataButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                deleteTrainingData();
            }
        });

        startPredictionButton = (Button) findViewById(R.id.startPredicting);
        startPredictionButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startPrediction();
            }
        });

        stopPredictionButton = (Button) findViewById(R.id.stopPredicting);
        stopPredictionButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                stopPrediction();
            }
        });

        getPredictedValuesButton = (Button) findViewById(R.id.getPredicted);
        getPredictedValuesButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                getPrediction();
            }
        });
    }
    public void openCollectTrainingDataActivity(){
        Intent intent = new Intent(this, CollectTrainingDataActivity.class);
        startActivity(intent);
    }

    public void buildModel() {
        callBuildModel = apiService.BuildModel();
        callBuildModel.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Process of building machine learning model stopped: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    public void deleteTrainingData() {
        callDeleteTrainingData = apiService.DeleteTrainingData();
        callDeleteTrainingData.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Process of deleting training data has been done: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    public void startPrediction() {
        callStartPrediction = apiService.StartPredictingOnTestData();
        callStartPrediction.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Process of prediction have been started: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    public void stopPrediction() {
        callStopPrediction = apiService.StopPredictingOnTestData();
        callStopPrediction.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Process of prediction have been stoped: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    public void getPrediction() {
        callGetPredictedValue = apiService.GetPredictedValue();
        callGetPredictedValue.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Predicted value have been retrieved: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }
}
