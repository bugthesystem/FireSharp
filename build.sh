build() {
    mono .nuget/NuGet.exe install NUnit.Runners
    mono .nuget/NuGet.exe restore FireSharp.sln

    xbuild /t:Rebuild FireSharp.sln

    if [[ $? != 0 ]] ; then
        build_failed
    else
        build_succeeded
    fi
}

build_failed() {
    print_status "BUILD FAILED"
    return 1
}

build_succeeded() {
    print_status "BUILD SUCCEEDED"
    run_tests
}

run_tests() {
    print_status "RUNNING TESTS"

    if [ -z "$USE_SYSTEM_NUNIT_CONSOLE" ]; then
        RUNNER_PATH="packages/NUnit.Runners.2.6.4/tools"
    else
        RUNNER_PATH="/usr/lib/nunit"
    fi
    NUNIT_ADDT_ARGS=""

    if [[ $NUNIT_RUN != "" ]]; then
        NUNIT_RUN_CSV=""
        for RUN in $NUNIT_RUN
        do
            NUNIT_RUN_CSV="$NUNIT_RUN_CSV,$RUN"
        done
        NUNIT_ADDT_ARGS="$NUNIT_ADDT_ARGS -run $NUNIT_RUN_CSV"
    fi

    mono ${RUNNER_PATH}/nunit-console.exe \
        FireSharp.Tests/bin/Debug/FireSharp.Tests.dll \
        $NUNIT_ADDT_ARGS

    local test_result=$?
    
    if [[ "${test_result}" != 0 ]] ; then
        tests_failed
    else
        tests_passed
    fi
}

tests_failed() {
    print_status "TESTS FAILED"
    return 2
}

tests_passed() {
    print_status "TESTS PASSED"
}

print_status() {
    echo ""
    echo "*** $1 ***"
    echo ""
}

NUNIT_RUN=$*
build

EXIT_CODE=$?
echo "Exit [$EXIT_CODE]"
exit $EXIT_CODE