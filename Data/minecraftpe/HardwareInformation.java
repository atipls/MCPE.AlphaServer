package com.mojang.minecraftpe;

import android.os.Build;
import android.os.Build.VERSION;
import java.io.File;
import java.io.FileFilter;
import java.util.regex.Pattern;

public class HardwareInformation {
    public static String getDeviceModelName() {
        String str = "";
        String manufacturer = Build.MANUFACTURER;
        String model = Build.MODEL;
        if (model.startsWith(manufacturer)) {
            return model.toUpperCase();
        }
        return manufacturer.toUpperCase() + " " + model;
    }

    public static String getAndroidVersion() {
        return VERSION.RELEASE;
    }

    public static String getCPU() {
        return Build.CPU_ABI;
    }

    public static int getNumCores() {
        try {
            return new File("/sys/devices/system/cpu/").listFiles(new FileFilter() {
                public boolean accept(File pathname) {
                    if (Pattern.matches("cpu[0-9]+", pathname.getName())) {
                        return true;
                    }
                    return false;
                }
            }).length;
        } catch (Exception e) {
            return 1;
        }
    }
}
