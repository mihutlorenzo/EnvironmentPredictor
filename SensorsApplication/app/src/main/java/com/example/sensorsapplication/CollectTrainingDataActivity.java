package com.example.sensorsapplication;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Toast;
import android.widget.ToggleButton;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CollectTrainingDataActivity extends AppCompatActivity {
    //AsyncHttpClient client;
    ApiInterface apiService;
    Call<String> callStartGatheringTraining;
    Call<String> callStopGatheringTraining;
    Call<String> callChangeEnvironmentState;
    private static final String TAG = MainActivity.class.getSimpleName();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_collect_training_data);
        ((RadioGroup) findViewById(R.id.toggleGroup)).setOnCheckedChangeListener(ToggleListener);
        //client = new AsyncHttpClient();
        apiService = ApiClient.getClient();


    }

    private void ChangeEnvironmentState(String newState) {
        callChangeEnvironmentState = apiService.SetUpEnvironmentState(newState);
        callChangeEnvironmentState.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Environment state changed to: " + messageFromServer);
                Toast.makeText(CollectTrainingDataActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    public void onRadioButtonClicked(View view) {
        // Is the button now checked?
        boolean checked = ((RadioButton) view).isChecked();

        // Check which radio button was clicked
        switch(view.getId()) {
            case R.id.light:
                if (checked)
                    ChangeEnvironmentState("LIGHT");
                    // Pirates are the best
                    break;
            case R.id.dark:
                if (checked)
                    ChangeEnvironmentState("DARK");
                    // Ninjas rule
                    break;
            case R.id.wet:
                if (checked)
                    ChangeEnvironmentState("WET");
                    // Ninjas rule
                    break;
            case R.id.dry:
                if (checked)
                    ChangeEnvironmentState("DRY");
                    // Ninjas rule
                    break;
            case R.id.hot:
                if (checked)
                    ChangeEnvironmentState("HOT");
                    // Ninjas rule
                    break;
            case R.id.cold:
                if (checked)
                    ChangeEnvironmentState("COLD");
                    // Ninjas rule
                    break;
            case R.id.noise:
                if (checked)
                    ChangeEnvironmentState("NOISE");
                    // Ninjas rule
                    break;
            case R.id.quiet:
                if (checked)
                    ChangeEnvironmentState("QUIET");
                    // Ninjas rule
                    break;
            default:
                break;
        }
    }


    static final RadioGroup.OnCheckedChangeListener ToggleListener = new RadioGroup.OnCheckedChangeListener() {
        @Override
        public void onCheckedChanged(final RadioGroup radioGroup, final int i) {
            for (int j = 0; j < radioGroup.getChildCount(); j++) {
                final ToggleButton view = (ToggleButton) radioGroup.getChildAt(j);
                view.setChecked(view.getId() == i);
            }
        }
    };

    public void onToggle(View view) {
        ((RadioGroup)view.getParent()).check(view.getId());
        boolean checked = ((ToggleButton) view).isChecked();

        // Check which radio button was clicked
        switch(view.getId()) {
            case R.id.startGatheringTrainingData:
                if (checked)
                    callStartGatheringTraining = apiService.StartGatherTrainingData();
                    callStartGatheringTraining.enqueue(new Callback<String>() {
                        @Override
                        public void onResponse(Call<String>call, Response<String> response) {
                            String messageFromServer = response.body();
                            Log.d(TAG, "Process of gathering training data started: " + messageFromServer);
                            Toast.makeText(CollectTrainingDataActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
                        }

                        @Override
                        public void onFailure(Call<String>call, Throwable t) {
                            // Log error here since request failed
                            Log.e(TAG, t.toString());
                        }
                    });
                    break;
            case R.id.stopGatheringTrainingData:
                if (checked)
                    callStopGatheringTraining = apiService.StopGatheringTrainingData();
                    callStopGatheringTraining.enqueue(new Callback<String>() {
                        @Override
                        public void onResponse(Call<String>call, Response<String> response) {
                            String messageFromServer = response.body();
                            Log.d(TAG, "Process of gathering training data stopped: " + messageFromServer);
                            Toast.makeText(CollectTrainingDataActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
                        }

                        @Override
                        public void onFailure(Call<String>call, Throwable t) {
                            // Log error here since request failed
                            Log.e(TAG, t.toString());
                        }
                    });
                    break;
            default:
                break;
        }
    }


}
